using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Expression.DesignSurface.UserInterface.ResourcePane;
using Microsoft.Expression.Extensibility;
using Microsoft.Expression.Framework.UserInterface;
using Microsoft.Expression.Utility.Extensions;
using Microsoft.Expression.Utility.Extensions.Enumerable;

namespace Pixelplastic.Expression.Blend.SortedResourcePane
{
    [Export(typeof(IPackage))]
    public class SortedResourcePane : IPackage
    {
        ObservableCollectionAggregator _resourceContainers;

        public void Load(IServices services)
        {
            var windowService = services.GetService<IWindowService>();
            windowService.MainWindow.ContentRendered += (s, a) => InitResourcePaneHook(windowService);
        }

        public void Unload() { }

        void InitResourcePaneHook(IWindowService windowService)
        {
            var resourcePalette = windowService.PaletteRegistry.PaletteRegistryEntries.FirstOrDefault(p => p.Name == "Designer_ResourcePane");
            if (resourcePalette == null) return;

            var resourcePane = resourcePalette.Content.FindVisualDescendentOfType<ResourcePane>();
            if (resourcePane == null) return;

            _resourceContainers = resourcePane.ResourceContainers as ObservableCollectionAggregator;
            if (_resourceContainers == null) return;

            _resourceContainers.CollectionChanged -= ResourceListChangedEventHandler;
            _resourceContainers.CollectionChanged += ResourceListChangedEventHandler;
        }

        void ResourceListChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            _resourceContainers.CollectionChanged -= ResourceListChangedEventHandler;
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(SortResourceList));
        }

        void SortResourceList()
        {
            if (((IList)_resourceContainers).Count > 0)
            {
                var currentItems = _resourceContainers
                    .OfType<object>()
                    .OrderBy(i => i.ToString())
                    .ToArray().ToList();

                currentItems.OfType<ResourceContainer>().ForEach(SortResourceItems);

                _resourceContainers.Clear();
                _resourceContainers.AddCollection(currentItems);
            }

            _resourceContainers.CollectionChanged += ResourceListChangedEventHandler;
        }

        void OnResourceItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var resourceItems = sender as ObservableCollection<ResourceItem>;
            if (resourceItems == null || resourceItems.Count == 0) return;

            Action sortResourceItems = () => SortResourceItems(resourceItems.First().Container);
            Application.Current.Dispatcher.BeginInvoke(sortResourceItems);
        }

        void SortResourceItems(ResourceContainer resourceContainer)
        {
            resourceContainer.ResourceItems.CollectionChanged -= OnResourceItemsChanged;

            var resourceItems = resourceContainer.ResourceItems.OrderBy(ResourceItemKey).ToList();
            resourceContainer.ResourceItems.Clear();
            resourceItems.ForEach(resourceContainer.ResourceItems.Add);

            resourceContainer.ResourceItems.CollectionChanged += OnResourceItemsChanged;
        }

        string ResourceItemKey(ResourceItem arg)
        {
            if (arg is DataTemplateResourceItem) return ((DataTemplateResourceItem) arg).Key;
            if (arg is StyleResourceItem) return ((StyleResourceItem) arg).Key;
            if (arg is ControlTemplateResourceItem) return ((ControlTemplateResourceItem) arg).Key;
            if (arg is ItemsPanelTemplateResourceItem) return ((ItemsPanelTemplateResourceItem)arg).Key;
            
            return String.Empty;
        }
    }
}
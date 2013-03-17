using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Threading;
using Microsoft.Expression.DesignSurface.UserInterface.ResourcePane;
using Microsoft.Expression.Extensibility;
using Microsoft.Expression.Framework.UserInterface;

namespace Pixelplastic.Expression.Blend.SortedResourcePane
{
    [Export(typeof(IPackage))]
    public class SortedResourcePane : IPackage
    {
        ObservableCollectionAggregator _resourceContainers;

        public void Load(IServices services)
        {
            var windowService = services.GetService<IWindowService>();
            InitResourcePaneHook(windowService);
        }

        public void Unload() { }

        void InitResourcePaneHook(IWindowService windowService)
        {
            var resourcePalette = windowService.PaletteRegistry.PaletteRegistryEntries.FirstOrDefault(p => p.Name == "Designer_ResourcePane");
            if (resourcePalette == null) return;

            var resourcePane = resourcePalette.Content as ResourcePane;
            if (resourcePane == null) return;

            _resourceContainers = resourcePane.ResourceContainers as ObservableCollectionAggregator;
            if (_resourceContainers == null) return;

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

                _resourceContainers.Clear();
                _resourceContainers.AddCollection(currentItems);
            }

            _resourceContainers.CollectionChanged += ResourceListChangedEventHandler;
        }
    }
}
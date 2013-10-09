# SortedResourcePane Extension for Microsoft Expression Blend

Did you ever noticed the unsorted list of resource files in the `Resources` pane in Expression Blend? This tiny extension will automatically sort the entries on any change.

## Warning

This software was not tested in deep by now (but will be soon). Thus use it AS IS and be aware of possible errors in Expression Blend -- even if I did not see any issues by time of writing. To disable this extension, simply remove the .dll from the extensions folder and restart Blend.

## Requirements

* Expression Blend 4 w/ SP1 (4.0.20901.0)
* Expression Blend Preview for Silverlight 5 (4.1.20402.0)
* Expression Blend for Visual Studio 2012 (5.0.40218.0)

The following assemblies must exist in the root folder of Expression Blend:

    Microsoft.Expression.DesignSurface.dll
    Microsoft.Expression.Extensibility.dll
    Microsoft.Expression.Framework.dll

## Installation

### Download precompiled assembly

* [SortedResourcePane.Blend4.Extension-0.2.0.zip](http://pixelplastic.de/dl/SortedResourcePane.Blend4.Extension-0.2.0.zip) 
* [SortedResourcePane.Blend5.Extension-0.2.0.zip](http://pixelplastic.de/dl/SortedResourcePane.Blend5.Extension-0.2.0.zip) 
* [SortedResourcePane.Blend2012.Extension-0.3.0.zip](http://pixelplastic.de/dl/SortedResourcePane.Blend2012.Extension-0.3.0.zip) 

Extract the containing .dll to the Expression Blend Extensions folder.

* for Blend 4: `%ProgramFiles%\Microsoft Expression\Blend 4\Extensions\`
* for Blend 5: `%ProgramFiles%\Microsoft Expression\Blend Preview for Silverlight 5\Extensions\`
* for Blend 2012: `%ProgramFiles%\Microsoft Visual Studio 11.0\Blend\Extensions\`

_You may need to create the `Extensions` folder._

### Build it on your own

Simply build the solution. It will automatically try to copy the binary `SortedResourcePane.Blend#.Extension.dll` to the Expression Blend Extensions folder. This means you need Administrator privileges to do so - or simply modify target folder ACLs.


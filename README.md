# FreeSource_TabLocale

FreeSource_TabLocale module is a simple pages localization component module for **DNN 9** ([DotNetNuke](https://github.com/dnnsoftware)).

This project built with [Christoc's DotNetNuke Module and Theme Development Template](https://github.com/ChrisHammond/DNNTemplates).

## Getting started

Install and source packages can be downloaded from the releases page on GitHub.

Drag the module to your page and start with experimenting!

![dnnfreessurcetablocale](https://user-images.githubusercontent.com/3435332/44077104-1d5b986c-9fd5-11e8-9aff-5ae8ff240fa6.gif)

## Prerequisite

- DNN v.09.2.0.366 and above

## Usage

Once you have installed the module, you can configure the translation from the module.

### DDRMenu NodeManipulator Example

```cs
using DotNetNuke.Entities.Portals;
using DotNetNuke.Web.DDRMenu;
using FreeSource.Modules.TabLocale.Components;
using System;
using System.Collections.Generic;

namespace FreeSource.Modules.TabLocale.Skin
{
    public class NodeManipulator : INodeManipulator
    {
        private Dictionary<int, LocalizedTabInfo> _localizedTabList;

        public Dictionary<int, LocalizedTabInfo> LocalizedTabList
        {
            get
            {
                if (_localizedTabList == null)
                {
                    var dict = new Dictionary<int, LocalizedTabInfo>();
                    var localizedTabs = TabLocaleController.GetLocalizedTabs(true);

                    foreach (var localizedTab in localizedTabs)
                    {
                        dict.Add(localizedTab.TabID, localizedTab);
                    }

                    _localizedTabList = dict;
                }
                return _localizedTabList;
            }
        }

        List<MenuNode> INodeManipulator.ManipulateNodes(List<MenuNode> nodes, PortalSettings portalSettings)
        {
            LocalizedNodes(ref nodes);
            return nodes;
        }

        void LocalizedNodes(ref List<MenuNode> nodes)
        {
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    try
                    {
                        LocalizedTabInfo localizedTabInfo = null;
                        if (LocalizedTabList.TryGetValue(node.TabId, out localizedTabInfo))
                        {
                            string localizedTabName = localizedTabInfo.TabLocalizedTabName;
                            if (!string.IsNullOrEmpty(localizedTabName))
                            {
                                node.Text = localizedTabName;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    // recursive localize nodes
                    if (node.HasChildren())
                    {
                        List<MenuNode> childNodes = node.Children;
                        LocalizedNodes(ref childNodes);
                        node.Children = childNodes;
                    }
                }
            }
        }
    }
}
```
		
## Development Project References

- [Visual Studio 2017 IDE](https://visualstudio.microsoft.com/downloads/)
- [DNN v.09.2.0.366](https://github.com/dnnsoftware/Dnn.Platform/releases/tag/v9.2.0)
- [Christoc's DotNetNuke Module Template](https://github.com/ChrisHammond/DNNTemplates)

## Contributing

If you'd like to contribute, please fork the repository and make changes as you'd like. Pull requests are warmly welcome.

## License

FreeSource_TabLocale is released under the MIT license.

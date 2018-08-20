/*
' Copyright (c) 2018  nicholashew@users.noreply.github.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.UI.Skins;
using FreeSource.Modules.TabLocale.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace FreeSource.Modules.TabLocale
{
    public partial class Breadcrumb : SkinObjectBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    var settings = new PortalSettingsRepository().GetSettings<BreadcrumbsSettings>(PortalSettings.PortalId);
                    var nodes = GetLocalisedNodes();
                    var markup = new StringBuilder();

                    if (settings != null)
                    {
                        if (nodes.Any())
                        {
                            bool hasSeperator = string.IsNullOrEmpty(settings.SeperatorTemplate);
                            int lastIndex = nodes.Count - 1;

                            markup.Append(settings.HeaderTemplate);

                            for (int i = 0; i < nodes.Count; i++)
                            {
                                var node = nodes[i];
                                string itemMarkup = settings.ItemTemplate.Replace("[url]", node.Link).Replace("[name]", node.Text);
                                markup.Append(itemMarkup);

                                if (i <= lastIndex && hasSeperator)
                                {
                                    markup.Append(settings.SeperatorTemplate);
                                }
                            }

                            markup.Append(settings.FooterTemplate);
                        }
                        else
                        {
                            markup.Append(settings.EmptyTemplate);
                        }
                    }

                    litOutput.Text = markup.ToString();
                }
                catch (Exception)
                {
                    //ignore
                }
            }
        }

        private List<MenuNode> GetLocalisedNodes()
        {
            var nodes = new List<MenuNode>();
            var breadcrumbs = PortalSettings.ActiveTab?.BreadCrumbs;
            int activeTabID = PortalSettings.ActiveTab?.TabID ?? -1;

            // Add home root
            if (breadcrumbs.Count > 0 && ((TabInfo)breadcrumbs[0]).TabID != PortalSettings.HomeTabId)
            {
                TabInfo homeTab = TabController.Instance.GetTab(PortalSettings.HomeTabId, PortalSettings.PortalId);
                nodes.Add(new MenuNode
                {
                    Link = homeTab.FullUrl,
                    Text = TabLocaleController.TryGetLocalizedTabName(homeTab.TabID, homeTab.TabName),
                    IsActive = activeTabID == homeTab.TabID
                });
            }

            // Add breadcrumb nodes
            for (int level = 0; level <= breadcrumbs.Count - 1; level++)
            {
                var tabInfo = (TabInfo)breadcrumbs[level];
                nodes.Add(new MenuNode
                {
                    Link = tabInfo.FullUrl,
                    Text = TabLocaleController.TryGetLocalizedTabName(tabInfo.TabID, tabInfo.TabName),
                    IsActive = activeTabID == tabInfo.TabID
                });
            }

            return nodes;
        }

        internal struct MenuNode
        {
            public string Link;
            public string Text;
            public bool IsActive;
        }
    }
}
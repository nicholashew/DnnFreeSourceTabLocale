/*
' Copyright (c) 2018 nicholashew@users.noreply.github.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeSource.Modules.TabLocale.Components
{
    public class TabLocaleController
    {
        #region Constructor

        private readonly ITabLocaleRepository repository;

        public TabLocaleController(ITabLocaleRepository htmlTextRepository)
        {
            Requires.NotNull("htmlTextRepository", htmlTextRepository);
            repository = htmlTextRepository;
        }

        public TabLocaleController() : this(TabLocaleRepository.Instance) { }

        #endregion

        public IEnumerable<TabLocale> GetAll() => repository.GetAll();

        public TabLocale GetByID(int id) => repository.GetById(id);

        public TabLocale GetByTabID(int tabId, string cultureCode) => repository.GetByTab(tabId, cultureCode);

        public IEnumerable<TabLocale> GetListByTabID(int tabId) => repository.GetAll(tabId);

        public int AddTabLocale(int tabId, string cultureCode, string tabName, string title, string description, string keywords, string pageHeadText, int createdBy)
        {
            var tab = new TabLocale
            {
                TabID = tabId,
                CultureCode = cultureCode,
                TabName = tabName,
                Title = title,
                Description = description,
                KeyWords = keywords,
                PageHeadText = pageHeadText,
                CreatedByUserId = createdBy,
                CreatedOnDate = DateTime.Now,
                LastModifiedByUserId = createdBy,
                LastModifiedOnDate = DateTime.Now
            };
            return repository.Add(tab);
        }

        public void UpdateTabLocale(int tabId, string cultureCode, string tabName, string title, string description, string keywords, string pageHeadText, int modifiedBy)
        {

            var tab = repository.GetByTab(tabId, cultureCode);
            if (tab != null)
            {
                tab.TabName = tabName;
                tab.Title = title;
                tab.Description = description;
                tab.KeyWords = keywords;
                tab.PageHeadText = pageHeadText;
                tab.LastModifiedByUserId = modifiedBy;
                tab.LastModifiedOnDate = DateTime.Now;
                repository.Update(tab);
            }
            else
            {
                AddTabLocale(tabId, cultureCode, tabName, title, description, keywords, pageHeadText, modifiedBy);
            }
        }

        public bool DeleteTabLocale(int tabId, string cultureCode)
        {
            try
            {
                var tab = repository.GetByTab(tabId, cultureCode);
                if (tab != null)
                {
                    repository.Delete(tabId, cultureCode);
                    ClearCache();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void ClearCache()
        {
            DataCache.ClearCache(Constants.CachePrefix + "_" + PortalSettings.Current.PortalId);
        }

        #region Static methods

        /// <summary>
        /// Loads localized TabInfos from the actual visible tabs for a given portal id. 
        /// System Tabs and Admin Tabs are excluded from the result set.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<LocalizedTabInfo> GetLocalizedTabs(bool fromCache)
        {
            string cacheKey = string.Format(Constants.LocalizedTabsCacheKey, PortalSettings.Current.PortalId);
            var cacheObject = fromCache ? DataCache.GetCache(cacheKey) : null;

            if (cacheObject != null)
            {
                return (List<LocalizedTabInfo>)cacheObject;
            }
            else
            {
                var localizedTabs = new List<LocalizedTabInfo>();
                var allTabLocales = new TabLocaleController().GetAll().ToList();
                //Dictionary<int, TabInfo> tabInfos = TabController.Instance.GetTabsByPortal(PortalSettings.Current.PortalId);
                Dictionary<int, TabInfo> tabInfos = TabController.Instance.GetUserTabsByPortal(PortalSettings.Current.PortalId);
                
                foreach (TabInfo dnnTab in tabInfos.Values)
                {                    
                    var tabInfo = new LocalizedTabInfo(dnnTab.Clone());

                    if (allTabLocales != null && allTabLocales.Count > 0)
                        tabInfo.LocaleList = allTabLocales.Where(x => x.TabID == tabInfo.TabID).ToList();

                    localizedTabs.Add(tabInfo);
                }

                if (fromCache)
                {
                    DataCache.SetCache(cacheKey, localizedTabs);
                }

                return localizedTabs;
            }
        }

        /// <summary>
        /// Matches a dnn core tabinfo object to the available localized tabs.
        /// if a match is available, the matching localized tab is return. 
        /// If no match is found, return a new LocalizedTabInfo instance from the dnn core tabinfo object
        /// </summary>
        /// <param name="tabInfo"></param>
        /// <param name="localizedTabs"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        internal static LocalizedTabInfo MatchLocaleTabToTab(TabInfo tabInfo, List<LocalizedTabInfo> localizedTabs)
        {
            if (tabInfo == null || !localizedTabs.Any())
            {
                return null;
            }

            var localizedTab = localizedTabs.SingleOrDefault(x => x.TabID == tabInfo.TabID);
            return localizedTab ?? new LocalizedTabInfo(tabInfo);
        }

        /// <summary>
        /// Get localized TabInfo
        /// </summary>
        /// <param name="tabInfo"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static LocalizedTabInfo GetLocalizedTab(TabInfo tabInfo, bool fromCache = true)
        {
            LocalizedTabInfo localizedTab = MatchLocaleTabToTab(tabInfo, GetLocalizedTabs(fromCache));
            return localizedTab ?? new LocalizedTabInfo(tabInfo);
        }

        public static LocalizedTabInfo GetLocalizedTab(int tabId, int portalId, bool fromCache)
        {
            TabInfo tabInfo = TabController.Instance.GetTab(tabId, portalId);

            if (tabInfo == null)
            {
                return null;
            }
            else if (fromCache)
            {
                return GetLocalizedTab(tabInfo, true);
            }
            else
            {
                var tabController = new TabLocaleController();
                var localizedTab = new LocalizedTabInfo(tabInfo.Clone())
                {
                    LocaleList = tabController.GetListByTabID(tabId).ToList()
                };
                return localizedTab;
            }
        }

        public static string TryGetLocalizedTabName(int tabId, string defaultTabName)
        {
            LocalizedTabInfo localizedTab = GetLocalizedTab(tabId, PortalSettings.Current.PortalId, true);

            if (localizedTab != null)
            {
                string localizedTabName = localizedTab.TabLocalizedTabName;
                if (!string.IsNullOrEmpty(localizedTabName))
                {
                    return localizedTabName;
                }
            }

            return defaultTabName;
        }

        #endregion
    }
}

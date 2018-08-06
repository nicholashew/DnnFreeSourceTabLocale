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

using DotNetNuke.Entities.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeSource.Modules.TabLocale.Components
{
    [Serializable]
    public class LocalizedTabInfo : TabInfo
    {
        public List<TabLocale> LocaleList { get; set; }

        public LocalizedTabInfo(object clone) { }

        /// <summary>
        /// Create a new LocalizedTabInfo instance. This instance contains all information
        /// off the original tab, and localized where possible
        /// </summary>
        /// <param name="tab"></param>
        public LocalizedTabInfo(TabInfo tab)
        {
            LocaleList = new List<TabLocale>();

            // assign the property values
            TabID = tab.TabID;
            TabOrder = tab.TabOrder;
            PortalID = tab.PortalID;
            TabName = tab.TabName;
            IsVisible = tab.IsVisible;
            ParentId = tab.ParentId;
            Level = tab.Level;
            IconFile = tab.IconFile;
            DisableLink = tab.DisableLink;
            Title = tab.Title;
            Description = tab.Description;
            KeyWords = tab.KeyWords;
            IsDeleted = tab.IsDeleted;
            Url = tab.Url;
            SkinSrc = tab.SkinSrc;
            ContainerSrc = tab.ContainerSrc;
            TabPath = tab.TabPath;
            StartDate = tab.StartDate;
            EndDate = tab.EndDate;
            HasChildren = tab.HasChildren;
            SkinPath = tab.SkinPath;
            ContainerPath = tab.ContainerPath;
            BreadCrumbs = tab.BreadCrumbs;
            IsSuperTab = tab.IsSuperTab;
            PageHeadText = tab.PageHeadText;
            RefreshInterval = tab.RefreshInterval;
            IsSecure = tab.IsSecure;
            SkinDoctype = tab.SkinDoctype;
            UniqueId = tab.UniqueId;
            VersionGuid = tab.VersionGuid;
            LocalizedVersionGuid = tab.LocalizedVersionGuid;
            DefaultLanguageGuid = tab.DefaultLanguageGuid;
            SiteMapPriority = tab.SiteMapPriority;
        }

        public string GetCurrentCultureCode() => System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();

        public string TabLocalizedTabName
        {
            get
            {
                var objLocale = GetCurrentLocaleTab();
                if (objLocale != null && !string.IsNullOrEmpty(objLocale.TabName))
                {
                    return objLocale.TabName;
                }
                return LocalizedTabName;
            }
        }

        public string LocalizedTitle
        {
            get
            {
                var objLocale = GetCurrentLocaleTab();
                if (objLocale != null && !string.IsNullOrEmpty(objLocale.Title))
                {
                    return objLocale.Title;
                }
                return Title;
            }
        }

        public string LocalizedDescription
        {
            get
            {
                var objLocale = GetCurrentLocaleTab();
                if (objLocale != null && !string.IsNullOrEmpty(objLocale.Description))
                {
                    return objLocale.Description;
                }
                return Description;
            }
        }

        public string LocalizedKeywords
        {
            get
            {
                var objLocale = GetCurrentLocaleTab();
                if (objLocale != null && !string.IsNullOrEmpty(objLocale.KeyWords))
                {
                    return objLocale.KeyWords;
                }
                return KeyWords;
            }
        }

        public string LocalizedPageHeadText
        {
            get
            {
                var objLocale = GetCurrentLocaleTab();
                if (objLocale != null && !string.IsNullOrEmpty(objLocale.PageHeadText))
                {
                    return objLocale.PageHeadText;
                }
                return PageHeadText;
            }
        }

        public TabLocale GetCurrentLocaleTab()
        {
            return GetLocaleProperty(System.Threading.Thread.CurrentThread.CurrentUICulture.ToString());
        }

        public TabLocale GetLocaleProperty(string CultureCode)
        {
            if (LocaleList != null && LocaleList.Count > 0)
            {
                return LocaleList.FirstOrDefault(x => x.CultureCode == CultureCode);
            }
            return null;
        }
    }
}
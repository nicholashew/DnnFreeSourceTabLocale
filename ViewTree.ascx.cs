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

using DotNetNuke.Entities.Tabs;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using FreeSource.Modules.TabLocale.Components;
using FreeSource.Modules.TabLocale.controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FreeSource.Modules.TabLocale
{
    public partial class ViewTree : TabLocaleModuleBase
    {
        public int SelectedTabId
        {
            get
            {
                int.TryParse(tvPages.SelectedValue, out int selectedTabId);
                return selectedTabId;
            }
        }

        #region Events

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            btnClearCache.Click += btnClearCache_Click;
            btnListView.Click += btnListView_Click;
            lbtnToggleCompare.Click += lbtnToggleCompare_Click;
            ucEditLocaleTab.OnLanguageChanged += new EditLocaleTab.OnLanguageChangedHandler(ucEditLocaleTab_OnLanguageChanged);
            ucEditLocaleTab.OnSaveClicked += new EditLocaleTab.OnSaveClickedHandler(ucEditLocaleTab_OnSaveClicked);
            ucEditLocaleTab.OnDeleteClicked += new EditLocaleTab.OnDeleteClickedHandler(ucEditLocaleTab_OnDeleteClicked);
            ucEditLocaleTabCompare.OnLanguageChanged += new EditLocaleTab.OnLanguageChangedHandler(ucEditLocaleTabCompare_OnLanguageChanged);
            ucEditLocaleTabCompare.OnSaveClicked += new EditLocaleTab.OnSaveClickedHandler(ucEditLocaleTabCompare_OnSaveClicked);
            ucEditLocaleTabCompare.OnDeleteClicked += new EditLocaleTab.OnDeleteClickedHandler(ucEditLocaleTabCompare_OnDeleteClicked);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    ucEditLocaleTab.Initialize();
                    ucEditLocaleTabCompare.Initialize(true);
                    pnlCompare.Visible = false;
                    lbtnToggleCompare.Text = Localization.GetString("Compare", LocalResourceFile);
                    BindTreeView();
                    ShowTabInfos(SelectedTabId);
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void btnClearCache_Click(object sender, EventArgs e)
        {
            new TabLocaleController().ClearCache();
        }

        protected void btnListView_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }

        protected void lbtnToggleCompare_Click(object sender, EventArgs e)
        {
            pnlCompare.Visible = !pnlCompare.Visible;

            if (pnlCompare.Visible == false)
            {
                ucEditLocaleTabCompare.ClearTabInfo();
                lbtnToggleCompare.Text = Localization.GetString("Compare", LocalResourceFile);
            }
            else
            {
                LoadCompareTabInfo(SelectedTabId);
                lbtnToggleCompare.Text = Localization.GetString("CloseCompare", LocalResourceFile);
            }
        }

        protected void ucEditLocaleTab_OnLanguageChanged(object sender, EventArgs e)
        {
            ShowTabInfos(SelectedTabId);
        }

        protected void ucEditLocaleTab_OnSaveClicked(object sender, EventArgs e)
        {
            int tabId = SelectedTabId;
            string cultureCode = ucEditLocaleTab.SelectedLanguage;
            string tabName = ucEditLocaleTab.TabName;
            string title = ucEditLocaleTab.TabTitle;
            string description = ucEditLocaleTab.TabDescription;
            string keyWords = ucEditLocaleTab.TabKeyWords;
            string pageHeadText = ucEditLocaleTab.TabPageHeadText;

            if (SaveTabLocale(tabId, cultureCode, tabName, title, description, keyWords, pageHeadText))
            {
                LoadTabInfo(tabId);
            }
        }

        protected void ucEditLocaleTab_OnDeleteClicked(object sender, EventArgs e)
        {
            int tabId = SelectedTabId;
            if (DeleteTabLocale(tabId, ucEditLocaleTab.SelectedLanguage))
            {
                LoadTabInfo(tabId);
            }
        }

        protected void ucEditLocaleTabCompare_OnLanguageChanged(object sender, EventArgs e)
        {
            ShowTabInfos(SelectedTabId);
        }

        protected void ucEditLocaleTabCompare_OnSaveClicked(object sender, EventArgs e)
        {
            int tabId = SelectedTabId;
            string cultureCode = ucEditLocaleTabCompare.SelectedLanguage;
            string tabName = ucEditLocaleTabCompare.TabName;
            string title = ucEditLocaleTabCompare.TabTitle;
            string description = ucEditLocaleTabCompare.TabDescription;
            string keyWords = ucEditLocaleTabCompare.TabKeyWords;
            string pageHeadText = ucEditLocaleTabCompare.TabPageHeadText;

            if (SaveTabLocale(tabId, cultureCode, tabName, title, description, keyWords, pageHeadText))
            {
                LoadCompareTabInfo(tabId);
            }
        }

        protected void ucEditLocaleTabCompare_OnDeleteClicked(object sender, EventArgs e)
        {
            int tabId = SelectedTabId;
            if (DeleteTabLocale(tabId, ucEditLocaleTabCompare.SelectedLanguage))
            {
                LoadCompareTabInfo(tabId);
            }
        }
        
        protected void tvPages_SelectedNodeChanged(object sender, EventArgs e)
        {
            ShowTabInfos(SelectedTabId);
        }

        protected void tvPages_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            if (e.Node.ChildNodes.Count == 0)
            {
                PopulateTreeView(int.Parse(e.Node.Value), e.Node);
            }
        }

        #endregion

        #region Methods

        private bool IsTabVisible(TabInfo tab)
        {
            return !tab.IsDeleted && TabPermissionController.CanViewPage(tab);
        }

        private TabInfo GetTopParent(TabInfo tab)
        {
            if (tab.ParentId > 0)
            {
                return GetTopParent(TabController.Instance.GetTab(tab.ParentId, PortalId, true));
            }
            else
            {
                return tab;
            }
        }

        private bool IsAdminTab(TabInfo tab)
        {
            return tab.IsSuperTab || tab.TabID == PortalSettings.AdminTabId || GetTopParent(tab).TabID == PortalSettings.AdminTabId;
        }

        protected void BindTreeView()
        {
            tvPages.Nodes.Clear();
            PopulateTreeView(-1, null);
            tvPages.Nodes[0].Selected = true;
            tvPages.Visible = (tvPages.Nodes.Count > 0);
        }

        private void PopulateTreeView(int parentId, TreeNode parentNode)
        {
            List<TabInfo> tabs = TabController.GetTabsByParent(parentId, PortalId);

            foreach (var tab in tabs)
            {
                if (IsTabVisible(tab) && !IsAdminTab(tab))
                {
                    var node = new TreeNode
                    {
                        Text = tab.TabName,
                        Value = tab.TabID.ToString(CultureInfo.InvariantCulture),
                        PopulateOnDemand = true,
                        Expanded = !tab.HasChildren
                    };

                    if (parentId <= 0)
                    {
                        tvPages.Nodes.Add(node);
                    }
                    else
                    {
                        parentNode?.ChildNodes.Add(node);
                    }
                }
            }
        }

        protected bool SaveTabLocale(int tabId, string cultureCode, string tabName, string title, string description, string keyWords, string pageHeadText)
        {
            if (tabId > 0 && !string.IsNullOrEmpty(tabName.Trim()))
            {
                try
                {
                    var tabLocaleController = new TabLocaleController();
                    tabLocaleController.UpdateTabLocale(tabId, cultureCode, tabName, title, description, keyWords, pageHeadText, UserId);
                    return true;
                }
                catch (Exception ex)
                {
                    Exceptions.ProcessModuleLoadException(this, ex);
                }
            }
            return false;
        }

        private bool DeleteTabLocale(int tabId, string cultureCode)
        {
            return new TabLocaleController().DeleteTabLocale(tabId, cultureCode);
        }

        protected void ShowTabInfos(int tabId)
        {
            var dnnTab = TabController.Instance.GetTab(tabId, PortalSettings.PortalId);

            if (dnnTab != null)
            {
                LoadTabInfo(tabId);

                // Compare mode
                if (pnlCompare.Visible)
                {
                    LoadCompareTabInfo(tabId);
                }
            }
            else
            {
                ucEditLocaleTab.ClearTabInfo();
                ucEditLocaleTabCompare.ClearTabInfo();
            }
        }

        protected void LoadTabInfo(int tabId)
        {
            var locTab = new TabLocaleController().GetByTabID(tabId, ucEditLocaleTab.SelectedLanguage);
            if (locTab != null)
            {
                ucEditLocaleTab.SetTabInfo(locTab.TabName, locTab.Title, locTab.Description, locTab.KeyWords, locTab.PageHeadText, true, true);
            }
            else
            {
                ucEditLocaleTab.SetTabInfo("", "", "", "", "", true, false);
            }
        }

        protected void LoadCompareTabInfo(int tabId)
        {
            if (string.IsNullOrEmpty(ucEditLocaleTabCompare.SelectedLanguage))
            {
                var dnnTab = TabController.Instance.GetTab(tabId, PortalSettings.PortalId);
                ucEditLocaleTabCompare.SetTabInfo(dnnTab?.TabName, dnnTab?.Title, dnnTab?.Description, dnnTab?.KeyWords, dnnTab?.PageHeadText, false, false);
            }
            else
            {
                // Compare locale
                var locTab = new TabLocaleController().GetByTabID(tabId, ucEditLocaleTabCompare.SelectedLanguage);
                bool isEditable = !ucEditLocaleTab.SelectedLanguage.Equals(ucEditLocaleTabCompare.SelectedLanguage);
                if (locTab != null)
                {
                    ucEditLocaleTabCompare.SetTabInfo(locTab.TabName, locTab.Title, locTab.Description, locTab.KeyWords, locTab.PageHeadText, isEditable, isEditable);
                }
                else
                {
                    ucEditLocaleTabCompare.SetTabInfo("", "", "", "", "", isEditable, false);
                }
            }
        }

        #endregion
    }
}
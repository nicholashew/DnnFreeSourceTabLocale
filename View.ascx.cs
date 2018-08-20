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

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using FreeSource.Modules.TabLocale.Components;
using System;
using System.Collections;
using System.Linq;
using System.Web.UI.WebControls;

namespace FreeSource.Modules.TabLocale
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from FreeSource_TabLocaleModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : TabLocaleModuleBase //, IActionable
    {
        #region Events

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            btnClearCache.Click += btnClearCache_Click;
            btnBreadCrumbs.Click += btnBreadCrumbs_Click;
            ddlTabs.SelectedIndexChanged += ddlTabs_SelectedIndexChanged;
            ddlLocale.SelectedIndexChanged += ddlLocale_SelectedIndexChanged;
            ddlLocaleCompare.SelectedIndexChanged += ddlLocaleCompare_SelectedIndexChanged;
            repTabs.ItemDataBound += repTabs_ItemDataBound;
            repTabs.ItemCommand += new RepeaterCommandEventHandler(repTabs_ItemCommand);
            btnUpdateAll.Click += btnUpdateAll_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    Localization.LoadCultureDropDownList(ddlLocale, CultureDropDownTypes.EnglishName, System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                    Localization.LoadCultureDropDownList(ddlLocaleCompare, CultureDropDownTypes.EnglishName, PortalSettings.DefaultLanguage);
                    BindTabDropDownList();
                    BindTabs(-1);
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        private void repTabs_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Update":
                    try
                    {
                        var hfTabId = (HiddenField)e.Item.FindControl("hfTabId");
                        var txtTabName = (TextBox)e.Item.FindControl("txtTabName");
                        var txtTabTitle = (TextBox)e.Item.FindControl("txtTabTitle");
                        var txtTabDescription = (TextBox)e.Item.FindControl("txtTabDescription");
                        var txtTabKeyWords = (TextBox)e.Item.FindControl("txtTabKeyWords");
                        var txtTabPageHeadText = (TextBox)e.Item.FindControl("txtTabPageHeadText");

                        int tabId = int.Parse(hfTabId.Value);
                        if (tabId > 0 && !string.IsNullOrEmpty(txtTabName.Text.Trim()))
                        {
                            var tabLocaleController = new TabLocaleController();
                            tabLocaleController.UpdateTabLocale(tabId, ddlLocale.SelectedValue, txtTabName.Text, txtTabTitle.Text, txtTabDescription.Text, txtTabKeyWords.Text, txtTabPageHeadText.Text, UserId);

                            txtTabName.Style.Remove("border-color");
                            txtTabName.ToolTip = "";                
                        }
                        else
                        {
                            txtTabName.Style.Add("border-color", "Red");
                            txtTabName.ToolTip = Localization.GetString("notTranslated", LocalResourceFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        Exceptions.ProcessModuleLoadException(this, ex);
                    }
                    break;
            }
        }

        protected void btnClearCache_Click(object sender, EventArgs e) {
            new TabLocaleController().ClearCache();
        }

        protected void btnBreadCrumbs_Click(object sender, EventArgs e)
        {            
            Response.Redirect(EditUrl("BreadcrumbSettings"));
        }

        protected void btnUpdateAll_Click(object sender, EventArgs e)
        {
            try
            {
                SaveAll();
                BindTabs(Convert.ToInt32(ddlTabs.SelectedValue));
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void ddlLocale_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindTabs(Convert.ToInt32(ddlTabs.SelectedValue));
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void ddlTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindTabs(Convert.ToInt32(ddlTabs.SelectedValue));
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void ddlLocaleCompare_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindTabs(Convert.ToInt32(ddlTabs.SelectedValue));
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void repTabs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var tInfo = (LocalizedTabInfo)e.Item.DataItem;

                var lblTabName = (Label)e.Item.FindControl("lblTabName");
                var lblTabTitle = (Label)e.Item.FindControl("lblTabTitle");
                var lblTabDescription = (Label)e.Item.FindControl("lblTabDescription");
                var lblTabKeyWords = (Label)e.Item.FindControl("lblTabKeyWords");
                var lblTabPageHeadText = (Label)e.Item.FindControl("lblTabPageHeadText");

                var txtTabName = (TextBox)e.Item.FindControl("txtTabName");
                var txtTabTitle = (TextBox)e.Item.FindControl("txtTabTitle");
                var txtTabDescription = (TextBox)e.Item.FindControl("txtTabDescription");
                var txtTabKeyWords = (TextBox)e.Item.FindControl("txtTabKeyWords");
                var txtTabPageHeadText = (TextBox)e.Item.FindControl("txtTabPageHeadText");

                var lblCompareTabName = (Label)e.Item.FindControl("lblCompareTabName");
                var lblCompareTabTitle = (Label)e.Item.FindControl("lblCompareTabTitle");
                var lblCompareTabDescription = (Label)e.Item.FindControl("lblCompareTabDescription");
                var lblCompareTabKeyWords = (Label)e.Item.FindControl("lblCompareTabKeyWords");
                var lblCompareTabPageHeadText = (Label)e.Item.FindControl("lblCompareTabPageHeadText");

                var hfTabId = (HiddenField)e.Item.FindControl("hfTabId");

                hfTabId.Value = tInfo.TabID.ToString();

                if (lblTabName != null)
                    lblTabName.Text = tInfo.TabName;

                if (lblTabTitle != null)
                    lblTabTitle.Text = tInfo.Title;

                if (lblTabDescription != null)
                    lblTabDescription.Text = tInfo.Description;

                if (lblTabKeyWords != null)
                    lblTabKeyWords.Text = tInfo.KeyWords;

                if (lblTabPageHeadText != null)
                    lblTabPageHeadText.Text = tInfo.PageHeadText;

                var selectedLocale = tInfo.GetLocaleProperty(ddlLocale.SelectedValue);
                if (selectedLocale != null)
                {
                    txtTabName.Text = selectedLocale.TabName;
                    txtTabTitle.Text = selectedLocale.Title;
                    txtTabDescription.Text = selectedLocale.Description;
                    txtTabKeyWords.Text = selectedLocale.KeyWords;
                    txtTabPageHeadText.Text = selectedLocale.PageHeadText;
                }

                var compareLocale = tInfo.GetLocaleProperty(ddlLocaleCompare.SelectedValue);
                if (compareLocale != null)
                {
                    lblCompareTabName.Text = compareLocale.TabName;
                    lblCompareTabTitle.Text = compareLocale.Title;
                    lblCompareTabDescription.Text = compareLocale.Description;
                    lblCompareTabKeyWords.Text = compareLocale.KeyWords;
                    lblCompareTabPageHeadText.Text = compareLocale.PageHeadText;
                }
            }
        }

        #endregion

        private void BindTabDropDownList()
        {
            var tabs = TabController.GetPortalTabs(PortalSettings.PortalId, Null.NullInteger, false, "None-Specified", true, false, false, true, false);
            var tabList = new ArrayList();

            foreach (TabInfo info in tabs)
            {
                if (info.HasChildren && info.TabID != PortalSettings.AdminTabId)
                {
                    string dots = "";
                    for (int i = 0; i < info.Level; i++)
                    {
                        dots += "...";
                    }
                    tabList.Add(new TabInfo
                    {
                        TabName = dots + info.TabName,
                        TabID = info.TabID
                    });
                }
            }

            var rootTab = new TabInfo
            {
                TabName = Localization.GetString("Root", this.LocalResourceFile),
                TabID = -1
            };
            tabList.Insert(0, rootTab);

            ddlTabs.DataSource = tabList;
            ddlTabs.DataTextField = "TabName";
            ddlTabs.DataValueField = "TabId";
            ddlTabs.DataBind();
        }

        private void BindTabs(int parentId)
        {
            var localizedTabs = TabLocaleController.GetLocalizedTabs(false);

            if (parentId <= 0)
            {
                localizedTabs = localizedTabs.Where(t => t.Level == 0 && !t.IsDeleted && !t.IsSystem && t.TabID != PortalSettings.AdminTabId && t.ParentId != PortalSettings.AdminTabId).ToList();
            }
            else
            {
                localizedTabs = localizedTabs.Where(t => t.ParentId == parentId && !t.IsDeleted && !t.IsSystem && t.TabID != PortalSettings.AdminTabId && t.ParentId != PortalSettings.AdminTabId).ToList();
            }

            repTabs.DataSource = localizedTabs;
            repTabs.DataBind();
        }

        private void SaveAll()
        {
            string selectedLocale = ddlLocale.SelectedValue;

            if (!string.IsNullOrEmpty(selectedLocale))
            {
                foreach (RepeaterItem item in repTabs.Items)
                {
                    var hfTabId = (HiddenField)item.FindControl("hfTabId");
                    var txtTabName = (TextBox)item.FindControl("txtTabName");
                    var txtTabTitle = (TextBox)item.FindControl("txtTabTitle");
                    var txtTabDescription = (TextBox)item.FindControl("txtTabDescription");
                    var txtTabKeyWords = (TextBox)item.FindControl("txtTabKeyWords");
                    var txtTabPageHeadText = (TextBox)item.FindControl("txtTabPageHeadText");                                                           

                    int tabId = int.Parse(hfTabId.Value);
                    if (tabId > 0 && !string.IsNullOrEmpty(txtTabName.Text.Trim()))
                    {
                        var tabLocaleController = new TabLocaleController();
                        tabLocaleController.UpdateTabLocale(tabId, selectedLocale, txtTabName.Text, txtTabTitle.Text, txtTabDescription.Text, txtTabKeyWords.Text, txtTabPageHeadText.Text, UserId);          
                    }
                }
            }
        }

        //public ModuleActionCollection ModuleActions
        //{
        //    get
        //    {
        //        var actions = new ModuleActionCollection
        //            {
        //                {
        //                    GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
        //                    EditUrl(), false, SecurityAccessLevel.Edit, true, false
        //                }
        //            };
        //        return actions;
        //    }
        //}
    }
}
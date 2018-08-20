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

using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Services.Exceptions;
using FreeSource.Modules.TabLocale.Components;
using System;

namespace FreeSource.Modules.TabLocale
{
    public partial class BreadcrumbSettings : PortalModuleBase
    {
        #region Event Handlers
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            JavaScript.RequestRegistration(CommonJs.DnnPlugins);
            btnSubmit.Click += OnSubmitClick;
            btnCancel.Click += OnCancelClick;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                LoadSettings();
            }
        }

        protected void OnSubmitClick(object sender, EventArgs e)
        {
            UpdateSettings();
        }

        protected void OnCancelClick(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }
        #endregion

        #region Method
        public void LoadSettings()
        {
            try
            {
                var settings = new PortalSettingsRepository().GetSettings<BreadcrumbsSettings>(PortalSettings.PortalId);
                txtHeaderTemplate.Text = settings?.HeaderTemplate;
                txtItemTemplate.Text = settings?.ItemTemplate;
                txtSeperatorTemplate.Text = settings?.SeperatorTemplate;
                txtFooterTemplate.Text = settings?.FooterTemplate;
                txtEmptyTemplate.Text = settings?.EmptyTemplate;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void UpdateSettings()
        {
            try
            {
                var settings = new BreadcrumbsSettings
                {
                    HeaderTemplate = txtHeaderTemplate.Text.Trim(),
                    ItemTemplate = txtItemTemplate.Text.Trim(),
                    SeperatorTemplate = txtSeperatorTemplate.Text.Trim(),
                    FooterTemplate = txtFooterTemplate.Text.Trim(),
                    EmptyTemplate = txtEmptyTemplate.Text.Trim()
                };

                var repo = new PortalSettingsRepository();
                repo.SaveSettings(settings, PortalSettings.PortalId);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
        #endregion
    }
}
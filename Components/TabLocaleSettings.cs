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

using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using System;
using System.Web.Script.Serialization;

namespace FreeSource.Modules.TabLocale.Components
{
    /// <summary>
    /// PortalSettings interface
    /// </summary>
    public interface IPortalSettings
    {
    }

    /// <summary>
    /// BreadcrumbsModuleSettings provides a strongly typed list of properties used by 
    /// the Breadcrumbs portal settings.
    /// </summary>
    [Serializable]
    public class BreadcrumbsSettings : IPortalSettings
    {
        public string HeaderTemplate { get; set; }

        public string ItemTemplate { get; set; }

        public string SeperatorTemplate { get; set; }

        public string FooterTemplate { get; set; }

        public string EmptyTemplate { get; set; }
    }

    /// <summary>
    /// The <see cref="PortalSettingsRepository"/> used for storing and retrieving <see cref="IPortalSettings"/>
    /// </summary>
    public class PortalSettingsRepository
    {
        private readonly IModuleController _moduleController;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortalSettingsRepository"/> class.</summary>
        /// </summary>
        public PortalSettingsRepository()
        {
            _moduleController = ModuleController.Instance;
        }

        public T GetSettings<T>(int portalID) where T : IPortalSettings, new()
        {
            var settings = new T();
            string settingName = Constants.ModuleSettingsPrefix + settings.GetType().Name;
            var jsonSettings = PortalController.GetPortalSetting(settingName, portalID, "");
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(jsonSettings);
        }

        public void SaveSettings<T>(T settings, int portalID) where T : IPortalSettings
        {
            Requires.NotNull("settings", settings);
            Requires.NotNull("portalID", portalID);

            string settingName = Constants.ModuleSettingsPrefix + settings.GetType().Name;
            var serializer = new JavaScriptSerializer();
            var jsonSettings = serializer.Serialize(settings);

            PortalController.UpdatePortalSetting(portalID, settingName, jsonSettings);
        }
    }
}
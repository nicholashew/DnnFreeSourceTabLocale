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

using DotNetNuke.ComponentModel.DataAnnotations;
using System;
using System.Web.Caching;

namespace FreeSource.Modules.TabLocale.Components
{
    [Serializable]
    [TableName("FreeSource_TabLocale")]
    [PrimaryKey("ID", AutoIncrement = true)]
    [Cacheable("FreeSource_TabLocale_Items", CacheItemPriority.Default, 20)]
    [Scope("TabID")]
    public class TabLocale
    {
        public int ID { get; set; }

        public int TabID { get; set; }
        
        public string CultureCode { get; set; }

        public string TabName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string KeyWords { get; set; }

        public string PageHeadText { get; set; }

        public int CreatedByUserId { get; set; }

        public int LastModifiedByUserId { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public DateTime LastModifiedOnDate { get; set; }
    }
}

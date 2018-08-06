/*
' Copyright (c) 2016 nicholashew@users.noreply.github.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Data;
using DotNetNuke.Framework;

namespace FreeSource.Modules.TabLocale.Components
{
    public class TabLocaleRepository : ServiceLocator<ITabLocaleRepository, TabLocaleRepository>, ITabLocaleRepository
    {
        protected override Func<ITabLocaleRepository> GetFactory()
        {
            return () => new TabLocaleRepository();
        }

        public int Add(TabLocale tabLocale)
        {
            Requires.NotNull("tabLocale", tabLocale);
            Requires.PropertyNotNegative("tabLocale", "TabID", tabLocale.TabID);
            Requires.PropertyNotNullOrEmpty("tabLocale", "CultureCode", tabLocale.CultureCode);
            Requires.PropertyNotNullOrEmpty("tabLocale", "TabName", tabLocale.TabName);

            var tab = GetByTab(tabLocale.TabID, tabLocale.CultureCode);
            if (tab != null)
            {
                tab.TabName = tabLocale.TabName;
                tab.Title = tabLocale.Title;
                tab.Description = tabLocale.Description;
                tab.KeyWords = tabLocale.KeyWords;
                tab.PageHeadText = tabLocale.PageHeadText;
                tab.LastModifiedByUserId = tabLocale.LastModifiedByUserId;
                tab.LastModifiedOnDate = tabLocale.LastModifiedOnDate;
                Update(tab);
                return tab.ID;
            }
            else {
                using (IDataContext db = DataContext.Instance())
                {
                    var repo = db.GetRepository<TabLocale>();
                    repo.Insert(tabLocale);
                }
                return tabLocale.ID;
            }
        }

        public void Delete(int tabId, string cultureCode)
        {
            Requires.NotNegative("tabId", tabId);
            Requires.NotNullOrEmpty("cultureCode", cultureCode);

            using (IDataContext db = DataContext.Instance())
            {
                var repo = db.GetRepository<TabLocale>();
                repo.Delete("WHERE TabID = @0 AND CultureCode = @1", tabId, cultureCode);
            }
        }

        public TabLocale GetById(int id)
        {
            using (IDataContext db = DataContext.Instance())
            {
                var repo = db.GetRepository<TabLocale>();
                return repo.GetById(id);
            }
        }

        public TabLocale GetByTab(int tabId, string cultureCode) {
            using (IDataContext db = DataContext.Instance())
            {
                var repo = db.GetRepository<TabLocale>();
                return repo.Find("WHERE TabID = @0 AND CultureCode = @1", tabId, cultureCode).SingleOrDefault();
            }
        }

        public IQueryable<TabLocale> GetAll()
        {
            IQueryable<TabLocale> tabList = null;

            using (IDataContext db = DataContext.Instance())
            {
                var repo = db.GetRepository<TabLocale>();
                tabList = repo.Get().AsQueryable();
            }

            return tabList;
        }

        public IQueryable<TabLocale> GetAll(int tabId)
        {
            Requires.NotNegative("tabId", tabId);

            return GetAll().Where(x => x.TabID == tabId);
        }

        public IEnumerable<TabLocale> GetAll(string cultureCode)
        {
            Requires.NotNullOrEmpty("cultureCode", cultureCode);

            return GetAll().Where(x => x.CultureCode == cultureCode);
        }

        public void Update(TabLocale tabLocale)
        {
            Requires.NotNull("tabLocale", tabLocale);
            Requires.PropertyNotNegative("tabLocale", "ID", tabLocale.ID);
            Requires.PropertyNotNegative("tabLocale", "TabID", tabLocale.TabID);
            Requires.PropertyNotNullOrEmpty("tabLocale", "CultureCode", tabLocale.CultureCode);
            Requires.PropertyNotNullOrEmpty("tabLocale", "TabName", tabLocale.TabName);

            using (IDataContext db = DataContext.Instance())
            {
                var repo = db.GetRepository<TabLocale>();
                repo.Update(tabLocale);
            }
        }
    }
}
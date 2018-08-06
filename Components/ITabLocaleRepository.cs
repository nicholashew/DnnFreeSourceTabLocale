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

using System.Collections.Generic;
using System.Linq;

namespace FreeSource.Modules.TabLocale.Components
{
    public interface ITabLocaleRepository
    {
        int Add(TabLocale tabLocale);

        void Delete(int tabId, string cultureCode);

        TabLocale GetById(int id);

        TabLocale GetByTab(int tabId, string cultureCode);

        IQueryable<TabLocale> GetAll();

        IQueryable<TabLocale> GetAll(int tabId);

        IEnumerable<TabLocale> GetAll(string cultureCode);

        void Update(TabLocale tabLocale);
    }
}
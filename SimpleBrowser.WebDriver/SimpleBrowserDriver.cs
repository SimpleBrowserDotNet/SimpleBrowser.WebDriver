using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using SimpleBrowserWebDriver;
using SimpleBrowser;
using System.Collections.ObjectModel;

namespace SimpleDriver
{
    public class SimpleBrowserDriver : IWebDriver
    {
        Browser _my ;
        public SimpleBrowserDriver()
        {
            _my = new Browser();
        }
        #region IWebDriver Members

        public void Close()
        {
            throw new NotImplementedException();
        }

        public string CurrentWindowHandle
        {
            get { throw new NotImplementedException(); }
        }

        public IOptions Manage()
        {
            return new SimpleManage(this);
        }

        public INavigation Navigate()
        {
            return new SimpleNavigate(this);
        }

        public string PageSource
        {
            get { return _my.CurrentHtml; }
            set {}
        }

        public void Quit()
        {
            throw new NotImplementedException();
        }

        public ITargetLocator SwitchTo()
        {
            throw new NotImplementedException();
        }

        public string Title
        {
            get { return _my.Find("title", new object()).Value; }
        }

        public string Url
        {
            get
            {
                return _my.Url.ToString();
            }
            set
            {
                _my.Navigate(value);
            }
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<string> WindowHandles
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region ISearchContext Members

        public IWebElement FindElement(By by)
        {
            ISearchContext ctx = CreateSearchContext(_my);
            return by.FindElement(ctx);
        }

        private ISearchContext CreateSearchContext(Browser my)
        {
            ISearchContext ctx = new WebElement(my.Find("html", new object()));
            return ctx;
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            ISearchContext ctx = CreateSearchContext(_my);
            return by.FindElements(ctx);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}

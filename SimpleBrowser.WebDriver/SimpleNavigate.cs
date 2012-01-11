using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace SimpleBrowser.WebDriver
{
    public class SimpleNavigate : INavigation
    {
        SimpleBrowserDriver _my;
        public SimpleNavigate(SimpleBrowserDriver driver)
        {
            _my = driver;
        }


        #region INavigation Members

        public void Back()
        {
            throw new NotImplementedException();
        }

        public void Forward()
        {
            throw new NotImplementedException();
        }

        public void GoToUrl(Uri url)
        {
            GoToUrl(url.ToString());
        }

        public void GoToUrl(string url)
        {
            _my.Url = url;
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

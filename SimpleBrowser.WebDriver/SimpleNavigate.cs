using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace SimpleBrowser.WebDriver
{
    public class SimpleNavigate : INavigation
    {
        private readonly SimpleBrowserDriver _my;

        private readonly Stack<string> _urlHistory = new Stack<string>();

        private string _currentUrl; 

        public SimpleNavigate(SimpleBrowserDriver driver)
        {
            _my = driver;
        }


        #region INavigation Members

        public void Back()
        {
            _currentUrl = _urlHistory.Pop();
            _my.Url = _currentUrl;
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
            if (_currentUrl != null) _urlHistory.Push(_currentUrl);
            _currentUrl = url;

            _my.Url = _currentUrl;
        }

        public void Refresh()
        {
            _my.Url = _currentUrl;
        }

        #endregion
    }
}

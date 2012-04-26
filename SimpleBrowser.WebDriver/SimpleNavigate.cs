using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace SimpleBrowser.WebDriver
{
    public class SimpleNavigate : INavigation
    {
        private readonly IBrowser _browser;

        private readonly Stack<string> _urlHistory = new Stack<string>();

        private string _currentUrl; 

        public SimpleNavigate(IBrowser browser)
        {
            _browser = browser;
        }


        #region INavigation Members

        public void Back()
        {
            _currentUrl = _urlHistory.Pop();
            NavigateToCurrent();
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

            NavigateToCurrent();
        }

        public void Refresh()
        {
            NavigateToCurrent();
        }

        private void NavigateToCurrent()
        {
            _browser.Navigate(_currentUrl);
        }

        #endregion
    }
}

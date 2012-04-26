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

        private readonly List<string> _urlCache = new List<string>();

        private int _currentIndex = -1;

        public SimpleNavigate(IBrowser browser)
        {
            _browser = browser;
        }


        #region INavigation Members

        public void Back()
        {
            GoBack();
            NavigateToCurrent();
        }

        public void Forward()
        {
            if (_currentIndex >= _urlCache.Count - 1) return;
            GoForward();
            NavigateToCurrent();
        }

        public void GoToUrl(Uri url)
        {
            if (url == null) return;
            GoToUrl(url.ToString());
        }

        public void GoToUrl(string url)
        {
            if (url == null) return;
            PushUrlToHistory(url);
            NavigateToCurrent();
        }

        public void Refresh()
        {
            NavigateToCurrent();
        }

        #endregion

        #region Private methods

        private string CurrentUrl
        {
            get { return _urlCache[_currentIndex]; }
        }

        private void PushUrlToHistory(string url)
        {
            if (_currentIndex != _urlCache.Count - 1)
            {
                var countToRemove = _urlCache.Count - 1 - _currentIndex;
                _urlCache.RemoveRange(_currentIndex+1, countToRemove);
            }
            _urlCache.Add(url);
            ++_currentIndex;
        }

        private void GoBack()
        {
            if (_currentIndex > 0) --_currentIndex;
        }

        private void GoForward()
        {
            if (_currentIndex != -1) ++_currentIndex;
        }

        private void NavigateToCurrent()
        {
            if (_currentIndex != -1)
                _browser.Navigate(CurrentUrl);
        }

        #endregion
    }
}

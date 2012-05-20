using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBrowser.WebDriver
{
    public class BrowserWrapper : IBrowser
    {
        Browser _my;
        public BrowserWrapper()
        {
            _my = new Browser();
			_my.RequestLogged += new Action<Browser, HttpRequestLog>(HandleRequestLogged);
		}
        public BrowserWrapper(Browser b)
        {
            _my = b;
			_my.RequestLogged += new Action<Browser, HttpRequestLog>(HandleRequestLogged);
        }


        #region IBrowser Members

        public string CurrentHtml
        {
            get { return _my.CurrentHtml; }
        }

        public IHtmlResult Find(string query, object param)
        {
            return new HtmlResultWrapper(_my.Find(query, param));
        }

        public Uri Url
        {
            get { return _my.Url; }
        }

        public void Navigate(string value)
        {
            _my.Navigate(value);
        }

		public void NavigateBack()
		{
			_my.NavigateBack();
		}

		public void NavigateForward()
		{
			_my.NavigateForward();
		}

		#endregion

		public event Action<Browser, HttpRequestLog> RequestLogged;
		private void HandleRequestLogged(Browser b, HttpRequestLog req)
		{
			if (this.RequestLogged != null)
			{
				this.RequestLogged(b, req);
			}
		}

	}
}


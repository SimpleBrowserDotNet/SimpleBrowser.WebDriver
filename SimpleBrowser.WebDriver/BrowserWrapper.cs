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
		public IHtmlResult Select(string query)
		{
			return new HtmlResultWrapper(_my.Select(query));
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



		public string WindowHandle
		{
			get { return _my.WindowHandle; }
		}

		public IEnumerable<IBrowser> Browsers
		{
			get { return _my.Windows.Select(b => new BrowserWrapper(b)); }
		}
		public IEnumerable<IBrowser> Frames
		{
			get { return _my.Frames.Select(b => new BrowserWrapper(b)); }
		}
		public void Close()
		{
			_my.Close();
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


		public KeyStateOption KeyState
		{
			get
			{
				return _my.KeyState;
			}
			set
			{
				_my.KeyState = value;
			}
		}

	}
}


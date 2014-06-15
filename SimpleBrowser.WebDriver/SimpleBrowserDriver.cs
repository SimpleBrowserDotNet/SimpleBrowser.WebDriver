using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using SimpleBrowser.WebDriver;
using SimpleBrowser;
using System.Collections.ObjectModel;

namespace SimpleBrowser.WebDriver
{
	public class SimpleBrowserDriver : IWebDriver, IHasInputDevices
	{
		IBrowser _my;
		public SimpleBrowserDriver()
		{
			_my = new BrowserWrapper();
		}
		public SimpleBrowserDriver(IBrowser browser)
		{
			_my = browser;
		}
		#region IWebDriver Members

		public void Close()
		{
			_my.Close();
			this.Dispose();
		}

		public string CurrentWindowHandle
		{
			get { return _my.WindowHandle; }
		}

		public IOptions Manage()
		{
			return new SimpleManage(this);
		}

		public INavigation Navigate()
		{
			return new SimpleNavigate(_my);
		}

		public string PageSource
		{
			get { return _my.CurrentHtml; }
			set { }
		}

		public void Quit()
		{
			this.Close();
		}

		public ITargetLocator SwitchTo()
		{
			return new SimpleTargetLocator(_my);
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
			get
			{
				return new ReadOnlyCollection<string>(_my.Browsers.Select(b => b.WindowHandle).ToList());
			}
		}

		#endregion

		#region ISearchContext Members

		public IWebElement FindElement(By by)
		{
			ISearchContext ctx = CreateSearchContext(_my);
			IWebElement result = by.FindElement(ctx);
			return result;
		}

		private ISearchContext CreateSearchContext(IBrowser my)
		{
			ISearchContext ctx = new PageRoot(my);
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
			_my.Close();
		}

		#endregion

		#region IHasInputDevices Members

		private IKeyboard _keyboard;
		public IKeyboard Keyboard
		{
			get {
				if (_keyboard == null)
				{
					_keyboard = new SimpleKeyboard(_my);
				}
				return _keyboard;
			}
		}

		private IMouse _mouse;
		public IMouse Mouse
		{
			get
			{
				if (_mouse == null)
				{
					_mouse = new SimpleMouse(_my);
				}
				return _mouse;
			}
		}

		#endregion
	}
}

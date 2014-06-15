using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace SimpleBrowser.WebDriver
{
	public class SimpleManage : IOptions
	{
		SimpleBrowserDriver _my;
		public SimpleManage(SimpleBrowserDriver driver)
		{
			_my = driver;
		}
		#region IOptions Members

		public ICookieJar Cookies
		{
			get { throw new NotImplementedException(); }
		}

		public ITimeouts Timeouts()
		{
			throw new NotImplementedException();
		}

		public IWindow Window
		{
			get { throw new NotImplementedException(); }
		}

		#endregion
	}
}

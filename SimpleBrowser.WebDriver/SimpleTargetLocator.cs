using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace SimpleBrowser.WebDriver
{
	public class SimpleTargetLocator : ITargetLocator
	{
		public SimpleTargetLocator(IBrowser browser)
		{
			_browser = browser;
		}
		private IBrowser _browser = null;

		#region ITargetLocator Members

		public IWebElement ActiveElement()
		{
			throw new NotImplementedException();
		}

		public IAlert Alert()
		{
			throw new NotImplementedException();
		}

		public IWebDriver DefaultContent()
		{
			throw new NotImplementedException();
		}

		public IWebDriver Frame(IWebElement frameElement)
		{
			string windowHandle = frameElement.GetAttribute("SimpleBrowser.WebDriver:frameWindowHandle");
			return Frame(windowHandle);
		}

		public IWebDriver Frame(string frameName)
		{
			var frame = _browser.Frames.FirstOrDefault(b => b.WindowHandle == frameName);
			return new SimpleBrowserDriver(frame);
		}

		public IWebDriver Frame(int frameIndex)
		{
			var frame = _browser.Frames.ToList()[frameIndex];
			return new SimpleBrowserDriver(frame);
		}

		public IWebDriver Window(string windowName)
		{
			var window = Browser.Windows.FirstOrDefault(b => b.WindowHandle == windowName);
			return new SimpleBrowserDriver(new BrowserWrapper(window));
		}

		#endregion
	}
}

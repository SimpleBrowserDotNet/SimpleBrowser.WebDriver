using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace SimpleBrowser.WebDriver
{
	public class SimpleKeyboard : IKeyboard
	{
		private IBrowser _browser;
		public SimpleKeyboard(IBrowser browser)
		{
			_browser = browser;
		}
		#region IKeyboard Members

		public void PressKey(string keyToPress)
		{
			if (keyToPress == Keys.Control)
			{
				TryPress(KeyStateOption.Ctrl);
				return;
			}
		}


		public void ReleaseKey(string keyToRelease)
		{
			if (keyToRelease == Keys.Control)
			{
				TryRelease(KeyStateOption.Ctrl);
				return;
			}

		}

		public void SendKeys(string keySequence)
		{
			throw new NotImplementedException();
		}

		#endregion

		private void TryPress(KeyStateOption keyPressed)
		{
			_browser.KeyState |= keyPressed;
		}
		private void TryRelease(KeyStateOption keyReleased)
		{
			_browser.KeyState &= ~keyReleased;
		}

	
	}
}

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
			if (keyToPress == Keys.Shift)
			{
				TryPress(KeyStateOption.Shift);
				return;
			}
			if (keyToPress == Keys.Alt)
			{
				TryPress(KeyStateOption.Alt);
				return;
			}
			throw new InvalidOperationException("Only the keys Ctrl, Shift and Alt are supported");
		}


		public void ReleaseKey(string keyToRelease)
		{
			if (keyToRelease == Keys.Control)
			{
				TryRelease(KeyStateOption.Ctrl);
				return;
			}
			if (keyToRelease == Keys.Shift)
			{
				TryRelease(KeyStateOption.Shift);
				return;
			}
			if (keyToRelease == Keys.Alt)
			{
				TryRelease(KeyStateOption.Alt);
				return;
			}
			throw new InvalidOperationException("Only the keys Ctrl, Shift and Alt are supported");
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

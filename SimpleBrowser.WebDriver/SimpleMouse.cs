using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace SimpleBrowser.WebDriver
{
	public class SimpleMouse : IMouse
	{
		private IBrowser _browser;
		public SimpleMouse(IBrowser browser)
		{
			_browser = browser;
		}
		#region IMouse Members

		public void Click(OpenQA.Selenium.Interactions.Internal.ICoordinates where)
		{
			IWebElement elm = where.AuxiliaryLocator as IWebElement;
			if (elm != null)
			{
				elm.Click();
			}
		}

		public void ContextClick(OpenQA.Selenium.Interactions.Internal.ICoordinates where)
		{
			throw new NotImplementedException();
		}

		public void DoubleClick(OpenQA.Selenium.Interactions.Internal.ICoordinates where)
		{
			throw new NotImplementedException();
		}

		public void MouseDown(OpenQA.Selenium.Interactions.Internal.ICoordinates where)
		{
			throw new NotImplementedException();
		}

		public void MouseMove(OpenQA.Selenium.Interactions.Internal.ICoordinates where, int offsetX, int offsetY)
		{
		}

		public void MouseMove(OpenQA.Selenium.Interactions.Internal.ICoordinates where)
		{
		}

		public void MouseUp(OpenQA.Selenium.Interactions.Internal.ICoordinates where)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

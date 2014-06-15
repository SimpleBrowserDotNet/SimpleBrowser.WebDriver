using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions.Internal;

namespace SimpleBrowser.WebDriver
{
	class SimpleCoordinates : ICoordinates
	{
		private IWebElement _elm;
		public SimpleCoordinates(IWebElement elm)
		{
			_elm = elm;
		}

		#region ICoordinates Members

		public object AuxiliaryLocator
		{
			get { return _elm; }
		}

		public System.Drawing.Point LocationInDom
		{
			get { return new Point(); }
		}

		public System.Drawing.Point LocationInViewport
		{
			get { return new Point(); }
		}

		public System.Drawing.Point LocationOnScreen
		{
			get { return new Point(); }
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System.Xml.Linq;
using SimpleBrowser;
using System.Collections.ObjectModel;
using System.Xml.XPath;
using System.Drawing;
using OpenQA.Selenium.Interactions.Internal;

namespace SimpleBrowser.WebDriver
{
	public class WebElement : IWebElement, ISearchContext, IFindsByLinkText, IFindsById, IFindsByName, IFindsByTagName, IFindsByClassName, IFindsByXPath, IFindsByPartialLinkText, IFindsByCssSelector, ILocatable
	{
		IHtmlResult _my;
		public WebElement(IHtmlResult about)
		{
			_my = about;
			if (_my.TotalElementsFound != 1) throw new InvalidSelectorException("Should return only one element");
		}
		#region IWebElement Members

		public void Clear()
		{
			_my.Value = "";
		}

		public void Click()
		{
			_my.Click();
		}

		public bool Displayed
		{
			get { return true; }
		}

		public bool Enabled
		{
			get { return !_my.Disabled; }
		}

		public string GetAttribute(string attributeName)
		{
			return _my.GetAttribute(attributeName);
		}

		public string GetCssValue(string propertyName)
		{
			throw new NotImplementedException();
		}

		public Point Location
		{
			get { throw new NotImplementedException(); }
		}

		public bool Selected
		{
			get
			{
				if (_my.XElement.Name == "option")
				{
					return _my.Checked;
				}
				if (_my.XElement.Name == "input")
				{
					return _my.Checked;
				}
				return false;
			}
		}

		public void SendKeys(string text)
		{
			_my.Value += text;
		}

		public Size Size
		{
			get { throw new NotImplementedException(); }
		}

		public void Submit()
		{
			_my.SubmitForm();
		}

		public string TagName
		{
			get { return _my.XElement.Name.LocalName; }
		}

		public string Text
		{
			get { return _my.DecodedValue; }
		}

		#endregion

		#region ISearchContext Members

		public IWebElement FindElement(By by)
		{
			return by.FindElement(this);
		}

		public ReadOnlyCollection<IWebElement> FindElements(By by)
		{
			return by.FindElements(this);
		}

		#endregion

		#region IFindsByLinkText Members

		public IWebElement FindElementByLinkText(string linkText)
		{
			return FindElementsByLinkText(linkText).FirstOrNoSuchElement(linkText);
		}

		public ReadOnlyCollection<IWebElement> FindElementsByLinkText(string linkText)
		{
			var results = SelectCss("a").Where(h => h.Value == linkText).Select(r => new WebElement(r));
			return new ReadOnlyCollection<IWebElement>(results.Cast<IWebElement>().ToList());
		}

		#endregion

		#region IFindsById Members

		public IWebElement FindElementById(string id)
		{
			return FindElementByCssSelector(String.Format("#{0}", id));
		}

		public ReadOnlyCollection<IWebElement> FindElementsById(string id)
		{
			return FindElementsByCssSelector(String.Format("#{0}", id));
		}

		#endregion

		#region IFindsByName Members

		public IWebElement FindElementByName(string name)
		{
			return FindElementsByName(name).FirstOrNoSuchElement(name);
		}

		public ReadOnlyCollection<IWebElement> FindElementsByName(string name)
		{
			return FindElementsByCssSelector(String.Format("*[name='{0}']", name));
		}

		#endregion

		#region IFindsByTagName Members

		public IWebElement FindElementByTagName(string tagName)
		{
			return FindElementByCssSelector(tagName);
		}

		public ReadOnlyCollection<IWebElement> FindElementsByTagName(string tagName)
		{
			return FindElementsByCssSelector(tagName);
		}

		#endregion

		#region IFindsByClassName Members

		public IWebElement FindElementByClassName(string className)
		{
			return FindElementByCssSelector(String.Format(".{0}", className));
		}

		public ReadOnlyCollection<IWebElement> FindElementsByClassName(string className)
		{
			return FindElementsByCssSelector(String.Format(".{0}", className));
		}

		#endregion

		#region IFindsByXPath Members

		public IWebElement FindElementByXPath(string xpath)
		{
			return FindElementsByXPath(xpath).FirstOrNoSuchElement(xpath);
		}

		public ReadOnlyCollection<IWebElement> FindElementsByXPath(string xpath)
		{
			var xpathnav = GetXmlRootNode().XPathSelectElements(xpath);
			var results = SelectCss("*")
					.Where(h => xpathnav.Contains(h.XElement))
					.Select(h => new WebElement(h))
					.Cast<IWebElement>()
					.ToList();
			return new ReadOnlyCollection<IWebElement>(results);
		}

		protected virtual XNode GetXmlRootNode()
		{
			return _my.XElement;
		}

		#endregion

		#region IFindsByPartialLinkText Members

		public IWebElement FindElementByPartialLinkText(string partialLinkText)
		{
			return FindElementsByPartialLinkText(partialLinkText).FirstOrNoSuchElement(partialLinkText);
		}

		public ReadOnlyCollection<IWebElement> FindElementsByPartialLinkText(string partialLinkText)
		{
			var results = SelectCss("a").Where(h => h.Value.Contains(partialLinkText)).Select(r => new WebElement(r));
			return new ReadOnlyCollection<IWebElement>(results.Cast<IWebElement>().ToList());
		}

		#endregion

		#region IFindsByCssSelector Members

		public IWebElement FindElementByCssSelector(string cssSelector)
		{
			return FindElementsByCssSelector(cssSelector).FirstOrNoSuchElement(cssSelector);
		}

		public ReadOnlyCollection<IWebElement> FindElementsByCssSelector(string cssSelector)
		{
			var r1 = SelectCss(cssSelector);
			var results = r1.Select(r => new WebElement(r)).ToList();
			return new ReadOnlyCollection<IWebElement>(results.Cast<IWebElement>().ToList());
		}
		protected virtual IHtmlResult SelectCss(string expr)
		{
			return _my.Select(expr);
		}

		#endregion

		#region ILocatable Members

		public ICoordinates Coordinates
		{
			get { return new SimpleCoordinates(this); }
		}

		public Point LocationOnScreenOnceScrolledIntoView
		{
			get { return new Point(); }
		}

		#endregion
	}
	static class Extensions
	{
		public static IWebElement FirstOrNoSuchElement(this ReadOnlyCollection<IWebElement> coll, string selectorInfo)
		{
			if (coll.Count > 0) return coll.First();
			throw new NoSuchElementException("No element was found for the expression: " + selectorInfo);
		}
	}

}

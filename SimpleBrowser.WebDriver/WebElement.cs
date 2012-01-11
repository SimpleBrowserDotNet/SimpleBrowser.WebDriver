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
    public class WebElement : IWebElement, ISearchContext, IFindsByLinkText, IFindsById, IFindsByName, IFindsByTagName, IFindsByClassName, IFindsByXPath, IFindsByPartialLinkText, IFindsByCssSelector
    {
        HtmlResult _my;
        public WebElement(HtmlResult about)
        {
            _my = about;
            if (_my.TotalElementsFound != 1) throw new InvalidSelectorException("Should return only one element");
        }
        #region IWebElement Members

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Click()
        {
            _my.Click();
        }

        public bool Displayed
        {
            get { throw new NotImplementedException(); }
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
            get { throw new NotImplementedException(); }
        }

        public void SendKeys(string text)
        {
            _my.Value = text;
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
            get { return _my.Value; }
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
            return FindElementsByLinkText(linkText).FirstOrDefault();
        }

        public ReadOnlyCollection<IWebElement> FindElementsByLinkText(string linkText)
        {
            var results = _my.Select("a").Where(h => h.Value == linkText).Select(r => new WebElement(r));
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
            return FindElementsByName(name).FirstOrDefault();
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
            return FindElementsByXPath(xpath).FirstOrDefault();
        }

        public ReadOnlyCollection<IWebElement> FindElementsByXPath(string xpath)
        {
            var xpathnav = _my.XElement.XPathSelectElements(xpath);
            var results = _my.Select("*")
                .Where(h => xpathnav.Contains(h.XElement))
                .Select(h => new WebElement(h))
                .Cast<IWebElement>()
                .ToList();
            return new ReadOnlyCollection<IWebElement>(results);
        }

        #endregion

        #region IFindsByPartialLinkText Members

        public IWebElement FindElementByPartialLinkText(string partialLinkText)
        {
            return FindElementsByPartialLinkText(partialLinkText).FirstOrDefault();
        }

        public ReadOnlyCollection<IWebElement> FindElementsByPartialLinkText(string partialLinkText)
        {
            var results = _my.Select("a").Where(h => h.Value.Contains(partialLinkText)).Select(r => new WebElement(r));
            return new ReadOnlyCollection<IWebElement>(results.Cast<IWebElement>().ToList());
        }

        #endregion

        #region IFindsByCssSelector Members

        public IWebElement FindElementByCssSelector(string cssSelector)
        {
            return FindElementsByCssSelector(cssSelector).FirstOrDefault();
        }

        public ReadOnlyCollection<IWebElement> FindElementsByCssSelector(string cssSelector)
        {
            var results = _my.Select(cssSelector).Select(r => new WebElement(r));
            return new ReadOnlyCollection<IWebElement>(results.Cast<IWebElement>().ToList());
        }

        #endregion

    }
}

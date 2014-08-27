using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBrowser.WebDriver
{
	public class PageRoot : WebElement
	{
		IBrowser _my;
		public PageRoot(IBrowser browser)
			: base(browser.Find("html", new object()))
		{
			_my = browser;
		}

		protected override System.Xml.Linq.XNode GetXmlRootNode()
		{
			var re = _my.Select("*");
			return re.XElement.Document;
		}
		protected override IHtmlResult SelectCss(string expr)
		{
			return _my.Select(expr);
		}
	}
}

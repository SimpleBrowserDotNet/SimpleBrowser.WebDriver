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

		protected override System.Xml.Linq.XNode GetRootNode()
		{
			var re = _my.Find("html", new object());
			return re.XElement.Parent;
		}
		protected override IHtmlResult SelectCss(string expr)
		{
			return _my.Select(expr);
		}
	}
}

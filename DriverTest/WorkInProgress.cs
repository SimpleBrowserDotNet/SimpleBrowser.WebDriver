using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SimpleBrowser;
using SimpleBrowser.WebDriver;
using OpenQA.Selenium;

namespace DriverTest
{
	[TestFixture]
	public class WorkInProgress
	{
		[Test]
		public void TestRussian()
		{
			Browser b = new Browser();
			b.Navigate("http://online3.anextour.ru/");

			var menuItem = b.Select(".menu_top div");
			Assert.That(menuItem.Value == "Турагентствам", "Russian text not found");

		}
		[Test]
		public void Issue9()
		{
			SimpleBrowserDriver driver = new SimpleBrowserDriver();
			driver.Navigate().GoToUrl("http://www.totaljobs.com");
			var txt = driver.FindElement(By.Id("left_0_txtKeywords"));
		}

	}
}

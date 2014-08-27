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
		[Test, Explicit]
		public void TestRussian()
		{
			Browser b = new Browser();
			b.Navigate("http://online3.anextour.ru/");

			var menuItem = b.Select(".mainmenu");
			Assert.That(menuItem.Value.Contains( "Поиск"), "Russian text not found");

		}

	}
}

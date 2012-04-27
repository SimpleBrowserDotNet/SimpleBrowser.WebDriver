using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SimpleBrowser;

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


			b.Navigate("http://www.funda.nl");

			b.Navigate("http://www.google.com");

		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SimpleBrowser.WebDriver;
using SimpleBrowser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace DriverTest
{
	[TestFixture]
	public class SimpleNavigateTests
	{

		private static Helper.BrowserWrapperWithLastRequest GetMockedBrowser()
		{
			Browser br = new Browser(Helper.GetAllways200RequestMocker(new List<Tuple<string, string>>()
				{
					Tuple.Create("link\\.htm$", "<html><body>Link: <a href=\"otherpage.htm\">link</a></body></html>"),
					Tuple.Create("^.*", "<html></html>"),
				}));
			
			Helper.BrowserWrapperWithLastRequest b = new Helper.BrowserWrapperWithLastRequest(br);
			return b;
		}


		[Test]
		public void NavigatingToUrlShouldDoIt()
		{
			var b = GetMockedBrowser();
			var dr = new SimpleBrowserDriver((IBrowser)b);
			var givenUrl = "http://www.a.com/";

			dr.Navigate().GoToUrl(givenUrl);

			Assert.AreEqual(dr.Url, givenUrl);
			Assert.AreEqual(b.LastRequest.Url.ToString(), givenUrl);
		}

		[Test]
		public void NavigatingBackwardsAfterSecondShouldNavigateToFirstUrl()
		{
			var b = GetMockedBrowser();
			var dr = new SimpleBrowserDriver((IBrowser)b);
			string[] givenUrls = { "http://www.a.com", "http://www.b.com" };
			foreach (string url in givenUrls)
			{
				dr.Navigate().GoToUrl(url);
			}

			dr.Navigate().Back();

			Assert.That(new Uri(dr.Url), Is.EqualTo(new Uri(givenUrls[0])));
		}

		[Test]
		public void NavigatingBackAndForthShouldNavigateToSecondUrl()
		{
			var b = GetMockedBrowser();
			var dr = new SimpleBrowserDriver((IBrowser)b);
			string[] givenUrls = { "http://www.a.com", "http://www.b.com" };
			foreach (string url in givenUrls)
			{
				dr.Navigate().GoToUrl(url);
			}

			dr.Navigate().Back();
			dr.Navigate().Forward();

			Assert.That(new Uri(dr.Url), Is.EqualTo(new Uri(givenUrls[1])));
		}

		[Test]
		public void GoingBackInTimeShouldntThrowExceptions()
		{
			var b = GetMockedBrowser();
			var dr = new SimpleBrowserDriver((IBrowser)b);
			var givenUrl = "http://www.a.com/";

			dr.Navigate().GoToUrl(givenUrl);
			dr.Navigate().Back();
			dr.Navigate().Back();
		}

		[Test]
		public void GoingBackInitiallyShouldntCallNavigate()
		{
			var b = GetMockedBrowser();
			var dr = new SimpleBrowserDriver((IBrowser)b);
			dr.Navigate().Back();

			Assert.IsNull(b.LastRequest);
		}

		[Test]
		public void GoingForwardInitiallyShouldntCallNavigate()
		{
			var b = GetMockedBrowser();
			var dr = new SimpleBrowserDriver((IBrowser)b);
			dr.Navigate().Forward();

			Assert.IsNull(b.LastRequest);
		}

		[Test]
		public void GoingToNullStringShouldntCallNavigate()
		{
			var b = GetMockedBrowser();
			var dr = new SimpleBrowserDriver((IBrowser)b);
			dr.Navigate().GoToUrl((string)null);

			Assert.IsNull(b.LastRequest);
		}


		[Test]
		public void GoingToNullUriShouldntCallNavigate()
		{
			var b = GetMockedBrowser();
			var dr = new SimpleBrowserDriver((IBrowser)b);
			dr.Navigate().GoToUrl((Uri)null);

			Assert.IsNull(b.LastRequest);
		}

		[Test]
		public void CallingRefreshShouldNavigateAgainToSame()
		{
			var b = GetMockedBrowser();
			var dr = new SimpleBrowserDriver((IBrowser)b);
			var givenUrl = "http://www.a.com/";
			dr.Navigate().GoToUrl(givenUrl);
			b.LastRequest = null;

			dr.Navigate().Refresh();

			Assert.NotNull(b.LastRequest);
		}

		[Test]
		public void GoingForwardInTimeShouldntCallNavigateAgain()
		{
			var b = GetMockedBrowser();
			var dr = new SimpleBrowserDriver((IBrowser)b);
			var givenUrl = "http://www.a.com/";
			dr.Navigate().GoToUrl(givenUrl);
			b.LastRequest = null;
			dr.Navigate().Forward();

			Assert.IsNull(b.LastRequest);
		}

		[Test]
		public void GoingBackAndForthWithNewUrlShouldCallNavigateWithNewUrl()
		{
			var b = GetMockedBrowser();
			var dr = new SimpleBrowserDriver((IBrowser)b);
			string[] givenUrls = { "http://www.a.com", "http://www.b.com" };
			foreach (string url in givenUrls)
			{
				dr.Navigate().GoToUrl(url);
			}
			dr.Navigate().Back();
			string newUrl = "http://www.c.com";

			dr.Navigate().GoToUrl(newUrl);

			Assert.That(b.LastRequest.Url, Is.EqualTo(new Uri(newUrl)));
		}
		[Test]
		public void CtrlClick_Should_Open_Link_In_Other_Window()
		{
			var b = GetMockedBrowser();
			var dr = new SimpleBrowserDriver((IBrowser)b);
			dr.Navigate().GoToUrl("http://www.a.com/link.htm");
			Assert.That(dr.WindowHandles.Count == 1);
			Assert.That(dr.Url == "http://www.a.com/link.htm");

			var link = dr.FindElement(By.LinkText("link"));
			Assert.NotNull(link);
			link.Click();
			Assert.That(dr.Url == "http://www.a.com/otherpage.htm");
			dr.Navigate().Back();
			Assert.That(dr.Url == "http://www.a.com/link.htm");
			link = dr.FindElement(By.LinkText("link"));

			Actions builder = new Actions(dr);
			builder.KeyDown(Keys.Control).Click(link).KeyUp(Keys.Control);
			var act = builder.Build();
			act.Perform();

			Assert.That(dr.Url == "http://www.a.com/link.htm");
			Assert.That(dr.WindowHandles.Count == 2);

		}
	}
}

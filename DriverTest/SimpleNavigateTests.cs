using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SimpleBrowser.WebDriver;
using SimpleBrowser;

namespace DriverTest
{
    [TestFixture]
    public class SimpleNavigateTests
    {

		private static Helper.BrowserWrapperWithLastRequest GetMockedBrowser()
		{
			Helper.BrowserWrapperWithLastRequest b = new Helper.BrowserWrapperWithLastRequest(new Browser( Helper.GetAllways200RequestMocker()));
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
    }
}

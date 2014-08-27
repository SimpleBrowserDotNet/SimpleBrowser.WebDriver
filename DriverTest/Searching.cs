using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using SimpleBrowser.WebDriver;
using Moq;
using SimpleBrowser;
using System.Reflection;
using System.IO;
using OpenQA.Selenium.Support.UI;

namespace DriverTest
{
    [TestFixture]
    public class Searching
    {
        [Test]
        public void UsingFindElements_Should_Convert_To_Correct_Jquery_Selector_Call()
        {
            Mock<IBrowser> mockBrowser;
						SetupElementSearch(By.ClassName("test"), out mockBrowser);
						mockBrowser.Verify(r => r.Select(".test"));

						SetupElementSearch(By.Id("test"), out mockBrowser);
						mockBrowser.Verify(r => r.Select("#test"));

						SetupElementSearch(By.CssSelector("div.blah>myid"), out mockBrowser);
						mockBrowser.Verify(r => r.Select("div.blah>myid"));

						SetupElementSearch(By.LinkText("test"), out mockBrowser);
						mockBrowser.Verify(r => r.Select("a"));
        }
		[Test]
        public void SearchingInKnownDocument()
        {
            Browser b = new Browser();
            b.SetContent(Helper.GetFromResources("DriverTest.GitHub.htm"));
            IWebDriver driver = new SimpleBrowserDriver(new BrowserWrapper(b));

            var iconSpans = driver.FindElements(By.CssSelector("span.icon"));
            Assert.That(iconSpans.Count == 4, "There should be 4 spans with class icon");

            var accountSettings = driver.FindElements(By.CssSelector("*[title~= Account]"));
            Assert.That(accountSettings.Count == 1 && accountSettings[0].Text == "Account Settings", "There should be 1 element with title containing the word Account");

            var topStuff = driver.FindElements(By.CssSelector("*[class|=top]"));
            Assert.That(topStuff.Count == 3 , "There should be 3 elements with class starting with top-");

			var issues_commentStuff = driver.FindElements(By.CssSelector(".issues_comment"));
			Assert.That(issues_commentStuff.Count == 16, "There should be 16 elements with class issues_comment");

			var h2s = driver.FindElements(By.CssSelector("h2"));
            Assert.That(h2s.Count == 8, "There should be 8 h2 elements");

            var titleContainingTeun = driver.FindElements(By.CssSelector("*[title*=Teun]"));
            Assert.That(titleContainingTeun.Count == 3, "There should be 3 elements with 'Teun' somewhere in the title attrbute");
        }
		[Test]
		public void FindingDuplicatesAndNotFinding()
		{
			Browser b = new Browser();
			b.SetContent(Helper.GetFromResources("DriverTest.GitHub.htm"));
			IWebDriver driver = new SimpleBrowserDriver(new BrowserWrapper(b));
			var anchor = driver.FindElement(By.TagName("a"));
			Assert.That(anchor.TagName == "a");

			Assert.Throws(typeof(NoSuchElementException), ()=>driver.FindElement(By.TagName("nosuchtag")));
		}
		[Test]
        public void UsingBrowserDirect()
        {
            Browser b = new Browser();
            b.SetContent(Helper.GetFromResources("DriverTest.GitHub.htm"));
            var found = b.Find("div", FindBy.Class, "issues_closed");
            Assert.That(found.TotalElementsFound == 3);
        }

		[Test]
		public void Searching_Html_Root_Element_Should_Work()
		{
			Browser b = new Browser();
			b.SetContent(Helper.GetFromResources("DriverTest.GitHub.htm"));
			IWebDriver driver = new SimpleBrowserDriver(new BrowserWrapper(b));

			var rootElement = driver.FindElements(By.TagName("html"));
			Assert.NotNull(rootElement);
			Assert.That(rootElement.Count > 0);
		}

		[Test]
		public void Repro_Issue24()
		{
			Browser b = new Browser();
			b.SetContent(Helper.GetFromResources("DriverTest.GitHub.htm"));
			IWebDriver driver = new SimpleBrowserDriver(new BrowserWrapper(b));

			var by = By.XPath("//input[contains(@id, 'repos')]");
			var tst = driver.FindElement(by);
		}
		private void SetupElementSearch(By by, out Mock<IBrowser> browserMock)
		{
			browserMock = new Mock<IBrowser>();
			var mock = new Mock<IHtmlResult>();
			var foundElement = new Mock<IHtmlResult>();
			var elmEnumerator = new Mock<IEnumerator<IHtmlResult>>();

			foundElement.Setup(h => h.TotalElementsFound).Returns(1);
			foundElement.Setup(h => h.GetEnumerator()).Returns(elmEnumerator.Object);
			elmEnumerator.Setup(e => e.Current).Returns(foundElement.Object);
			elmEnumerator.SetupSequence(e => e.MoveNext()).Returns(true).Returns(false);
			mock.Setup(h => h.TotalElementsFound).Returns(1);
			browserMock.Setup(browser => browser.Find("html", It.IsAny<object>())).Returns(mock.Object);
			browserMock.Setup(browser => browser.Select(It.IsAny<string>())).Returns(foundElement.Object);

			string url = "http://testweb.tst";
			SimpleBrowserDriver driver = new SimpleBrowserDriver(browserMock.Object);
			driver.Navigate().GoToUrl(url);
			driver.FindElements(by);

			browserMock.Verify(b => b.Navigate(url));
			browserMock.Verify(b => b.Find("html", It.IsAny<object>()));
		}

    }
}

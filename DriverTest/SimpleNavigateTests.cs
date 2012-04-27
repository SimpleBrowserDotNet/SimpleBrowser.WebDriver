using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SimpleBrowser.WebDriver;

namespace DriverTest
{
    [TestFixture]
    public class SimpleNavigateTests
    {
        public class MockedBrowser : IBrowser
        {
            public string GetCurrentHtml = null;
            public Uri GetUrl = null;
            public bool HasCalledFind = false;
            public bool HasCalledNavigate = false;
            public int HasCalledNavigateTimes = 0;
            public string NavigateValue = null;
            public string FindQuery = null;
            public object FindParam = null;

            public string CurrentHtml
            {
                get { return GetCurrentHtml; }
            }

            public Uri Url
            {
                get { return GetUrl; }
            }

            public IHtmlResult Find(string query, object param)
            {
                HasCalledFind = true;
                FindQuery = query;
                FindParam = param;
                return null;
            }

            public void Navigate(string value)
            {
                HasCalledNavigate = true;
                HasCalledNavigateTimes++;
                NavigateValue = value;
            }
        }

        private SimpleNavigate _simpleNavigate;

        private MockedBrowser _mockedBrowser;

        [SetUp]
        public void SetUp()
        {
            _mockedBrowser = new MockedBrowser();

            _simpleNavigate = new SimpleNavigate(_mockedBrowser);
        }

        [Test]
        public void NavigatingToUrlShouldDoIt()
        {
            var givenUrl = "http://www.a.com/";

            _simpleNavigate.GoToUrl(givenUrl);

            Assert.That(_mockedBrowser.HasCalledNavigate, Is.True);
            Assert.That(_mockedBrowser.NavigateValue, Is.EqualTo(givenUrl));
        }

        [Test]
        public void NavigatingBackwardsAfterSecondShouldNavigateToFirstUrl()
        {
            string[] givenUrls = {"http://www.a.com", "http://www.b.com"};
            foreach (string url in givenUrls)
            {
                _simpleNavigate.GoToUrl(url);
            }
            
            _simpleNavigate.Back();

            Assert.That(_mockedBrowser.NavigateValue, Is.EqualTo(givenUrls[0]));
        }

        [Test]
        public void NavigatingBackAndForthShouldNavigateToSecondUrl()
        {
            string[] givenUrls = { "http://www.a.com", "http://www.b.com" };
            foreach (string url in givenUrls)
            {
                _simpleNavigate.GoToUrl(url);
            }

            _simpleNavigate.Back();
            _simpleNavigate.Forward();

            Assert.That(_mockedBrowser.NavigateValue, Is.EqualTo(givenUrls[1]));
        }

        [Test]
        public void GoingBackInTimeShouldntThrowExceptions()
        {
            var givenUrl = "http://www.a.com/";

            _simpleNavigate.GoToUrl(givenUrl);
            _simpleNavigate.Back();
            _simpleNavigate.Back();
        }

        [Test]
        public void GoingBackInitiallyShouldntCallNavigate()
        {
            _simpleNavigate.Back();

            Assert.That(_mockedBrowser.HasCalledNavigate, Is.False);
        }

        [Test]
        public void GoingForwardInitiallyShouldntCallNavigate()
        {
            _simpleNavigate.Forward();

            Assert.That(_mockedBrowser.HasCalledNavigate, Is.False);
        }

        [Test]
        public void GoingToNullStringShouldntCallNavigate()
        {
            _simpleNavigate.GoToUrl(null as string);

            Assert.That(_mockedBrowser.HasCalledNavigate, Is.False);
        }

        [Test]
        public void GoingToNullUriShouldntCallNavigate()
        {
            _simpleNavigate.GoToUrl(null as Uri);

            Assert.That(_mockedBrowser.HasCalledNavigate, Is.False);
        }

        [Test]
        public void CallingRefreshShouldNavigateAgainToSame()
        {
            var givenUrl = "http://www.a.com/";
            _simpleNavigate.GoToUrl(givenUrl);

            _simpleNavigate.Refresh();
            
            Assert.That(_mockedBrowser.HasCalledNavigate, Is.True);
            Assert.That(_mockedBrowser.HasCalledNavigateTimes, Is.EqualTo(2));
        }

        [Test]
        public void GoingForwardInTimeShouldntCallNavigateAgain()
        {
            var givenUrl = "http://www.a.com/";
            _simpleNavigate.GoToUrl(givenUrl);

            _simpleNavigate.Forward();

            Assert.That(_mockedBrowser.HasCalledNavigateTimes, Is.EqualTo(1));
        }

        [Test]
        public void GoingBackAndForthWithNewUrlShouldCallNavigateWithNewUrl()
        {
            string[] givenUrls = { "http://www.a.com", "http://www.b.com" };
            foreach (string url in givenUrls)
            {
                _simpleNavigate.GoToUrl(url);
            }
            _simpleNavigate.Back();
            string newUrl = "http://www.c.com";

            _simpleNavigate.GoToUrl(newUrl);

            Assert.That(_mockedBrowser.NavigateValue, Is.EqualTo(newUrl));
        }
    }
}

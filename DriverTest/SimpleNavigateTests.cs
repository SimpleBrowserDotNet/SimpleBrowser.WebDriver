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
    }
}

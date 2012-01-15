using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using SimpleBrowser.WebDriver;

namespace DriverTest
{
    [TestFixture]
    public class Searching
    {
        [Test]
        public void SearchingByTagname_CShould_Convert_To_Correct_Jquery_Selector_Call()
        {
            SimpleBrowserDriver driver = new SimpleBrowserDriver(new FakeBrowser());
        }

    }
    public class FakeBrowser : IBrowser
    {
        #region IBrowser Members

        public string CurrentHtml
        {
            get { throw new NotImplementedException(); }
        }

        public Uri Url
        {
            get { throw new NotImplementedException(); }
        }

        public IHtmlResult Find(string query, object param)
        {
            throw new NotImplementedException();
        }

        public void Navigate(string value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

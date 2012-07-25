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
	public class WindowsAndFrames
	{
		[SetUp]
		public void Clear()
		{
			Browser.ClearWindows();
		}

		[Test]
		public void IframesShouldLoad()
		{
			IBrowser b = new BrowserWrapper(new Browser(Helper.GetAllways200RequestMocker(
				new List<Tuple<string, string>>()
				{
					Tuple.Create("^/frame", "<html></html>"),
					Tuple.Create("^.*", @"
										<html>
											<iframe src=""/frame""/><iframe src=""/frame""/>
										</html>"),
				}
				)));
			var dr = new SimpleBrowserDriver((IBrowser)b);
			b.Navigate("http://blah/");
			var test = dr.FindElement(By.TagName("html"));
			Assert.That(dr.WindowHandles.Count == 3);
			var firstFrame = dr.SwitchTo().Frame(0);
			Assert.IsNull(firstFrame.FindElement(By.TagName("iframe")));
		}
		[Test]
		public void OpeningOtherWindows()
		{
			IBrowser b = new BrowserWrapper(new Browser(Helper.GetAllways200RequestMocker(
				new List<Tuple<string, string>>()
				{
					Tuple.Create("^/otherpage", "<html></html>"),
					Tuple.Create("^.*", @"
										<html>
											<a href=""/otherpage"" target=""_blank"" id=""blanklink"">click</a>
										</html>"),
				}
			)));
			var dr = new SimpleBrowserDriver((IBrowser)b);
			dr.Navigate().GoToUrl("http://blah/");
			dr.FindElement(By.Id("blanklink")).Click();
			Assert.That(dr.Url == "http://blah/");
			Assert.That(dr.WindowHandles.Count == 2);
			string otherWindowName = dr.WindowHandles.First(n => n != dr.CurrentWindowHandle);
			var otherDr = dr.SwitchTo().Window(otherWindowName);
			Assert.That(otherDr.Url == "http://blah/otherpage");

			// click it again will create a thrid window
			dr.FindElement(By.Id("blanklink")).Click();
			Assert.That(dr.Url == "http://blah/");
			Assert.That(dr.WindowHandles.Count == 3);
		}
		[Test]
		public void FrameSwitchingUsingElementReference()
		{
			IBrowser b = new BrowserWrapper(new Browser(Helper.GetAllways200RequestMocker(
				new List<Tuple<string, string>>()
				{
					Tuple.Create("^/frame", "<html></html>"),
					Tuple.Create("^.*", @"
										<html>
											<iframe src=""/frame""/><iframe src=""/frame""/>
										</html>"),
				}
				)));
			var dr = new SimpleBrowserDriver((IBrowser)b);
			b.Navigate("http://blah/");
			var test = dr.FindElement(By.TagName("html"));
			Assert.That(dr.WindowHandles.Count == 3);
			var iframe = dr.FindElement(By.TagName("iframe"));
			var firstFrame = dr.SwitchTo().Frame(iframe);
			Assert.That(firstFrame.Url == "http://blah/frame");
		}


	}
}

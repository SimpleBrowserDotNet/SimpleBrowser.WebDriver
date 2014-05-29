using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SimpleBrowser;
using OpenQA.Selenium;
using SimpleBrowser.WebDriver;
using OpenQA.Selenium.Support.UI;
using System.Collections.Specialized;

namespace DriverTest
{
	[TestFixture]
	public class Form
	{
		[Test]
		public void UsingLinks()
		{
			Browser b = new Browser();
			b.SetContent(Helper.GetFromResources("DriverTest.SimpleForm.htm"));
			IWebDriver driver = new SimpleBrowserDriver(new BrowserWrapper(b));
			var link = driver.FindElement(By.CssSelector("a.clickLink"));
			string lastRequest = "";
			b.RequestLogged += (browser, logged) =>
			{
				Console.WriteLine("Request logged: " + logged.Url.ToString());
				lastRequest = logged.Url.AbsoluteUri;
			};
			link.Click();
			// this URL is intentionally non-existing. After the error we continue working on the same page
			Assert.That(lastRequest.Contains("wwwgooglecom/search"), "Link has resulted in unexpected request");

			b.SetContent(Helper.GetFromResources("DriverTest.SimpleForm.htm"));
			link = driver.FindElement(By.CssSelector("a.link-relative"));
			link.Click();
			Assert.That(lastRequest.Contains("/search"), "Link has resulted in unexpected request");
			
		}
		[Test]
        public void UsingSelectBoxes()
        {
            Browser b = new Browser();
            b.SetContent(Helper.GetFromResources("DriverTest.GitHub.htm"));
            IWebDriver driver = new SimpleBrowserDriver(new BrowserWrapper(b));
            var selectbox = driver.FindElement(By.Name("sel"));
            var box = new SelectElement(selectbox);
            Assert.That(box.SelectedOption.Text == "two");
            box.SelectByValue("3");
            Assert.That(box.SelectedOption.Text == "three");
            box.SelectByText("one");
            Assert.That(box.SelectedOption.Text == "one");

			selectbox = driver.FindElement(By.Name("sel_multi"));
			box = new SelectElement(selectbox);
			Assert.That(box.IsMultiple);
			box.SelectByValue("3");
			box.SelectByText("one");
			Assert.That(box.AllSelectedOptions.Count == 3);

		}
		[Test]
		public void UsingCheckboxes()
		{
			Browser b = new Browser();
			b.SetContent(Helper.GetFromResources("DriverTest.GitHub.htm"));
			IWebDriver driver = new SimpleBrowserDriver(new BrowserWrapper(b));
			var checkbox1 = driver.FindElement(By.CssSelector(".cb-container #first-checkbox"));
			var checkbox2 = driver.FindElement(By.CssSelector(".cb-container #second-checkbox"));
			Assert.That(checkbox1.Selected, "Checkbox 1 should be selected");
			Assert.That(!checkbox2.Selected, "Checkbox 2 should not be selected");
			checkbox2.Click();
			Assert.That(checkbox1.Selected, "Checkbox 1 should still be selected");
			Assert.That(checkbox2.Selected, "Checkbox 2 should be selected");
			var checkbox1Label = driver.FindElement(By.CssSelector("label[for=first-checkbox]"));
			Assert.NotNull(checkbox1Label, "Label not found");
			checkbox1Label.Click();
			Assert.That(checkbox2.Selected, "Checkbox 2 should still be selected");
			Assert.That(!checkbox1.Selected, "Checkbox 1 should be not selected");
			
		}
		[Test]
		public void UsingRadioButtons()
		{
			Browser b = new Browser();
			b.SetContent(Helper.GetFromResources("DriverTest.GitHub.htm"));
			IWebDriver driver = new SimpleBrowserDriver(new BrowserWrapper(b));
			var radio1 = driver.FindElement(By.CssSelector(".rb-container #first-radio"));
			var radio2 = driver.FindElement(By.CssSelector(".rb-container #second-radio"));
			var radio1Label = driver.FindElement(By.CssSelector("label[for=first-radio]"));
			Assert.That(radio1.Selected, "Radiobutton 1 should be selected");
			Assert.That(!radio2.Selected, "Radiobutton 2 should not be selected");
			radio2.Click();
			Assert.That(!radio1.Selected, "Radiobutton 1 should not be selected");
			Assert.That(radio2.Selected, "Radiobutton 2 should be selected");
			Assert.NotNull(radio1Label, "Label not found");
			radio1Label.Click();
			Assert.That(radio1.Selected, "Radiobutton 1 should be selected");
			Assert.That(!radio2.Selected, "Radiobutton 2 should be not selected");

		}
		[Test]
		public void UsingTextboxes()
		{
			Browser b = new Browser();
			b.SetContent(Helper.GetFromResources("DriverTest.GitHub.htm"));
			IWebDriver driver = new SimpleBrowserDriver(new BrowserWrapper(b));
			var textbox = driver.FindElement(By.CssSelector("#your-repos-filter"));
			Assert.NotNull(textbox, "Couldn't find textbox");
			Assert.That(textbox.Text == String.Empty, "Textbox without a value attribute should have empty text");
			textbox.SendKeys("test text");
			Assert.That(textbox.Text == "test text", "Textbox did not pick up sent keys");
			textbox.SendKeys(" more");
			Assert.That(textbox.Text == "test text more", "Textbox did not append second text");
			textbox.Clear();
			Assert.That(textbox.Text == String.Empty, "Textbox after Clear should have empty text");
		}

		[Test]
		public void UsingTextareas()
		{
			Browser b = new Browser();
			b.SetContent(Helper.GetFromResources("DriverTest.SimpleForm.htm"));
			IWebDriver driver = new SimpleBrowserDriver(new BrowserWrapper(b));
			var textbox = driver.FindElement(By.Name("textarea_a"));
			Assert.That(textbox != null);
			Assert.That(textbox.Text.Contains("\n"), "Textarea should not make line breaks coalesce into space");
		}

		[Test]
		public void UsingHtml5Inputs()
		{
			Browser b = new Browser();
			b.SetContent(Helper.GetFromResources("DriverTest.SimpleForm.htm"));
			IWebDriver driver = new SimpleBrowserDriver(new BrowserWrapper(b));
			var colorBox = driver.FindElement(By.Id("colorBox"));
			Assert.NotNull(colorBox, "Couldn't find colorbox");
			colorBox.SendKeys("ff0000");
			Assert.AreEqual(colorBox.Text, "ff0000", "Colorbox did not pick up sent keys");

			var form = driver.FindElement(By.CssSelector("form"));
			string lastRequest = "";
			b.RequestLogged += (browser, logged) =>
			{
				Console.WriteLine("Request logged: " + logged.Url.ToString());
				lastRequest = logged.Url.AbsoluteUri;
			};
			form.Submit();
			Assert.That(lastRequest.Contains("colorBox=ff0000"), "Color box not posted correctly");
		}

		[Test]
		public void SubmitGetForm()
		{
			Browser b = new Browser(Helper.GetAllways200RequestMocker());
			b.SetContent(Helper.GetFromResources("DriverTest.SimpleForm.htm"));
			IWebDriver driver = new SimpleBrowserDriver(new BrowserWrapper(b));
			var form = driver.FindElement(By.CssSelector("form"));
			string lastRequest = "";
			b.RequestLogged += (browser, logged) =>
				{
					Console.WriteLine("Request logged: " + logged.Url.ToString());
					lastRequest = logged.Url.AbsoluteUri;
				};
			form.Submit();
			Assert.That(lastRequest.Contains("radios=first"), "Radio buttons not in correct state");
			// NOTE: this line seems wrong: the line breaks in a textarea should remain preserved. But, XML parsing will remove this.
			//       What are the actual rules around this
			//Assert.That(lastRequest.Contains("textarea_a=This+is+a+full+text+part%0d%0awith"), "Textarea not posted correctly");

      driver.Navigate().Back();
      b.SetContent(Helper.GetFromResources("DriverTest.SimpleForm.htm"));
      form = driver.FindElement(By.CssSelector("form"));
      var firstRadio = driver.FindElement(By.Id("first-radio"));
			var firstRadioLabel = driver.FindElement(By.CssSelector("label[for=first-radio]"));
			var secondRadio = driver.FindElement(By.Id("second-radio"));
			secondRadio.Click();
			form.Submit();
			Assert.That(lastRequest.Contains("radios=second"), "Radio buttons not in correct state");

      b.SetContent(Helper.GetFromResources("DriverTest.SimpleForm.htm"));
      form = driver.FindElement(By.CssSelector("form"));
      firstRadioLabel = driver.FindElement(By.CssSelector("label[for=first-radio]"));
      firstRadioLabel.Click();
			form.Submit();
			Assert.That(lastRequest.Contains("radios=first"), "Radio buttons not in correct state");

      b.SetContent(Helper.GetFromResources("DriverTest.SimpleForm.htm"));
      form = driver.FindElement(By.CssSelector("form"));
      var selectBox = driver.FindElement(By.Id("optionsList"));
			var box = new SelectElement(selectBox);
			Assert.That(box.AllSelectedOptions.Count == 1, "First option should be selected in selectbox");
			form.Submit();
			Assert.That(lastRequest.Contains("optionsList=opt1"), "Selectbox not in correct state");

			Assert.That(!lastRequest.Contains("submitButton=button1"), "Value of submit button should not be posted");

      b.SetContent(Helper.GetFromResources("DriverTest.SimpleForm.htm"));
      form = driver.FindElement(By.CssSelector("form"));
      var submitButton = driver.FindElement(By.CssSelector("input[type=submit]"));
			submitButton.Click();
			Assert.That(lastRequest.Contains("submitButton=button1"), "Value of submit button not posted");
		}
		[Test]
		public void SubmitPostForm()
		{
			Browser b = new Browser(Helper.GetAllways200RequestMocker());
			b.SetContent(Helper.GetFromResources("DriverTest.SimplePostForm.htm"));
			IWebDriver driver = new SimpleBrowserDriver(new BrowserWrapper(b));
			var form = driver.FindElement(By.CssSelector("form"));
			NameValueCollection lastRequest = null;
			b.RequestLogged += (browser, logged) =>
			{
				Console.WriteLine("Request logged: " + logged.Url.ToString());
				lastRequest = logged.PostData;
			};
			form.Submit();
			Assert.That(lastRequest.AllKeys.Contains("radios") && lastRequest["radios"].Contains("first"), "Radio buttons not in correct state");
			// NOTE: this line seems wrong: the line breaks in a textarea should remain preserved. But, XML parsing will remove this.
			//       What are the actual rules around this
			//Assert.That(lastRequest.AllKeys.Contains("textarea_a") && lastRequest["textarea_a"].Contains("This is a full text part\r\nwith"), "Textarea not posted correctly");

      b.SetContent(Helper.GetFromResources("DriverTest.SimplePostForm.htm"));
      form = driver.FindElement(By.CssSelector("form"));
      var firstRadio = driver.FindElement(By.Id("first-radio"));
			var firstRadioLabel = driver.FindElement(By.CssSelector("label[for=first-radio]"));
			var secondRadio = driver.FindElement(By.Id("second-radio"));
			secondRadio.Click();
			form.Submit();
			Assert.That(lastRequest.AllKeys.Contains("radios") && lastRequest["radios"].Contains("second"), "Radio buttons not in correct state");

      b.SetContent(Helper.GetFromResources("DriverTest.SimplePostForm.htm"));
      form = driver.FindElement(By.CssSelector("form"));
      firstRadioLabel = driver.FindElement(By.CssSelector("label[for=first-radio]"));
      firstRadioLabel.Click();
			form.Submit();
			Assert.That(lastRequest.AllKeys.Contains("radios") && lastRequest["radios"].Contains("first"), "Radio buttons not in correct state");

      b.SetContent(Helper.GetFromResources("DriverTest.SimplePostForm.htm"));
      form = driver.FindElement(By.CssSelector("form"));
      var selectBox = driver.FindElement(By.Id("optionsList"));
			var box = new SelectElement(selectBox);
			Assert.That(box.AllSelectedOptions.Count == 1, "First option should be selected in selectbox");
			form.Submit();
			Assert.That(lastRequest.AllKeys.Contains("optionsList") && lastRequest["optionsList"].Contains("opt1"), "Selectbox not in correct state");


			Assert.That(!lastRequest.AllKeys.Contains("submitButton"), "Value of submit button should not be posted");

      b.SetContent(Helper.GetFromResources("DriverTest.SimplePostForm.htm"));
      var submitButton = driver.FindElement(By.CssSelector("input[type=submit]"));
			submitButton.Click();
			Assert.That(lastRequest.AllKeys.Contains("submitButton") && lastRequest["submitButton"].Contains("button1"), "Value of submit button not posted");

			Assert.That(!lastRequest.AllKeys.Contains("submitButton2"), "Value of submit button should not be posted");

      b.SetContent(Helper.GetFromResources("DriverTest.SimplePostForm.htm"));
      submitButton = driver.FindElement(By.CssSelector("button[type=submit]"));
			submitButton.Click();
			Assert.That(lastRequest.AllKeys.Contains("submitButton2") && lastRequest["submitButton2"].Contains("button2"), "Value of submit button not posted");
		}
		[Test]
		public void PostAspnetPostbackForm()
		{
			Browser b = new Browser(Helper.GetAllways200RequestMocker());
			b.SetContent(Helper.GetFromResources("DriverTest.PostbackForm.htm"));
			IWebDriver driver = new SimpleBrowserDriver(new BrowserWrapper(b));
			var form = driver.FindElement(By.CssSelector("form"));
			NameValueCollection lastRequest = null;
			b.RequestLogged += (browser, logged) =>
			{
				Console.WriteLine("Request logged: " + logged.Url.ToString());
				lastRequest = logged.PostData;
			};
			var postbackLink = driver.FindElement(By.Id("postbackLink"));
			postbackLink.Click();
			Assert.That(lastRequest.AllKeys.Contains("__EVENTTARGET") && lastRequest["__EVENTTARGET"].Contains("colorBox"), "colorBox was not indicated as the postback target");
		}
	}
}


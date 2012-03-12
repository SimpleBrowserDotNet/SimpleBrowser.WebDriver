using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SimpleBrowser;
using OpenQA.Selenium;
using SimpleBrowser.WebDriver;
using OpenQA.Selenium.Support.UI;

namespace DriverTest
{
	[TestFixture]
	public class Form
	{
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
		public void PostingForms()
		{
			Browser b = new Browser();
			b.SetContent(Helper.GetFromResources("DriverTest.SimpleForm.htm"));
			IWebDriver driver = new SimpleBrowserDriver(new BrowserWrapper(b));
			var form = driver.FindElement(By.CssSelector("form"));
			string lastRequest = "";
			b.RequestLogged += (browser, logged) =>
				{
					Console.WriteLine("Request logged: " + logged.Url.ToString());
					lastRequest = logged.Url.ToString();
				};
			form.Submit();
			Assert.That(lastRequest.Contains("radios=first"), "Radio buttons not in correct state");

			var firstRadio = driver.FindElement(By.Id("first-radio"));
			var firstRadioLabel = driver.FindElement(By.CssSelector("label[for=first-radio]"));
			var secondRadio = driver.FindElement(By.Id("second-radio"));
			secondRadio.Click();
			form.Submit();
			Assert.That(lastRequest.Contains("radios=second"), "Radio buttons not in correct state");

			firstRadioLabel.Click();
			form.Submit();
			Assert.That(lastRequest.Contains("radios=first"), "Radio buttons not in correct state");

		}

	}
}


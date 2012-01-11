using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using SimpleBrowser.WebDriver;

namespace DriverTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            // This is some really stupid testing code. I want to replace this by proper unit test and mock away 
            // the simplebrowser library and the internet using MoQ
            IWebDriver browser = new SimpleBrowserDriver();
            browser.Navigate().GoToUrl("http://localhost:3000/todo");
            string src = browser.PageSource;
            var inputBox = browser.FindElement(By.Id("new-todo"));
            Console.WriteLine(inputBox.TagName);
            var list = browser.FindElement(By.ClassName("content"));
            Console.WriteLine(list.Text);
            var header = browser.FindElement(By.TagName("h1"));
            Console.WriteLine(header.Text);
            inputBox = browser.FindElement(By.XPath("//*[@placeholder]"));
            inputBox.SendKeys("test");
            Console.WriteLine(inputBox.Text);
            inputBox = browser.FindElement(By.Name("somename"));
            Console.WriteLine(inputBox.Text);

            browser.Navigate().GoToUrl("http://www.funda.nl/koop");
            var firstThings = browser.FindElements(By.CssSelector("*[name!='description' ]"));
            firstThings = browser.FindElements(By.CssSelector("div[class ~= frst]"));
            firstThings = browser.FindElements(By.CssSelector("*[class ^= nav]"));

            var input = browser.FindElement(By.Name("PCPlaats"));
            input.SendKeys("Utrecht");
            input.Submit();
            Console.WriteLine(browser.Title);


            
            Console.ReadLine();
        }
    }
}

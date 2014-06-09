SimpleBrowser.WebDriver - Headless WebDriver for .NET
-----------------------------------------------------

###An implementation of a WebDriver that uses SimpleBrowser as its browser. 

WebDriver is part of the Selenium project (http://code.google.com/p/selenium/) and provides a framework for automated testing of websites and web applications. The WebDriver library is contributed to the community as open source by Google. It contains a number of drivers for several real world browsers (IE, FireFox, Chrome and many more) on several platforms. These browsers can be automated through a standard interface (IWebDriver) that is made available om several platforms (java, python, .NET, ruby, etc...).

It is usefull to do tests with real browsers, but for performance reasons it can also be very important to be able to test with an in-memory, light-weight browser. These browsers are called 'headless'. The most common headless browser in use is HtmlUnit, a java implementation. It is not easy to use this browser in-memory from a .NET context. This is where SimpleBrowser comes in (https://github.com/axefrog/SimpleBrowser). SimpleBrowser is a headless browser for the .NET platform. It is not as complete and mature as HtmlUnit, but it supports cookie-based sessions, HTTP/HTTPS, CSS selectors, etc. No javascript though, but frankly, if you need javascript support, you'll lose most of the performance gain of a headless browser anyway (because of all the extra requests you'll have to do).

SimpleBrowser.WebDriver allows C# developers to do selenium testing fast and in memory.

###Installation
The easiest way to get started is:

    Install-Package SimpleBrowser.WebDriver


###Sample:

    IWebDriver browser = new SimpleBrowserDriver();
    browser.Navigate().GoToUrl("http://www.funda.nl/koop");

    var input = browser.FindElement(By.Name("PCPlaats"));
    input.SendKeys("Utrecht");
    input.Submit();
    Console.WriteLine(browser.Title);

###More
[Developing with SimpleBrowser.WebDriver](https://github.com/Teun/SimpleBrowser.WebDriver/wiki/Developing-with-SimpleBrowser.WebDriver)

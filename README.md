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

###Tip: when you need to reference the binaries from a project with another version of WebDriver
The downloadable binaries of the SimpleBrowser.WebDriver will normally be compiled against a relatively recent version of WebDriver.dll. As WebDriver.dll is a strongnamed assembly, the reference to one version is completely incompatible with another version. So if your test library is compiled against a different version of the WebDriver assemblies, you will not be able to use the downloaded binaries for the driver. You can either compile the driver from code every time and so compile it against the same version of WebDriver or you can perform this trick in config:

    <configuration>
        <runtime>
            <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
                <dependentAssembly>
                    <assemblyIdentity name="WebDriver" publicKeyToken="1c2bd1631853048f" culture="neutral" />
                    <bindingRedirect oldVersion="2.13.0.0" newVersion="2.16.0.0"/>
                </dependentAssembly>
            </assemblyBinding>
       </runtime>
    </configuration>

This will cause the runtime to accept a newer version of the dll in you binaries. In this example you would have downloaded the binaries for the driver compiled against version 2.13 of WebDriver, while your tests are more up-to-date (2.16).
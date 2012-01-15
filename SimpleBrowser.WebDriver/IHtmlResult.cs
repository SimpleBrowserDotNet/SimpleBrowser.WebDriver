using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SimpleBrowser.WebDriver
{
    public interface IHtmlResult : IEnumerable<IHtmlResult>
    {

        string Value { get; set; }

        int TotalElementsFound { get; }

        void Click();

        bool Disabled { get; }

        string GetAttribute(string attributeName);

        void SubmitForm();

        IHtmlResult Select(string p);
        XElement XElement { get; }
    }
}

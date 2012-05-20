using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBrowser.WebDriver
{
    /// <summary>
    /// For testability we want to access the Browser object only through an interface
    /// This interface contains only the properties and methods that are actually used.
    /// </summary>
    public interface IBrowser
    {
        string CurrentHtml{get;}
        Uri Url { get; }
        IHtmlResult Find(string query, object param);
        void Navigate(string value);
		void NavigateBack();
		void NavigateForward();
	}
}

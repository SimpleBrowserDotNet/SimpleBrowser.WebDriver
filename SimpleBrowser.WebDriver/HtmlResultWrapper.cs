using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBrowser.WebDriver
{
	public class HtmlResultWrapper : IHtmlResult
	{
		private HtmlResult _htmlResult;

		public HtmlResultWrapper(HtmlResult htmlResult)
		{
			this._htmlResult = htmlResult;
		}

		#region IHtmlResult Members

		public string Value
		{
			get { return _htmlResult.Value; }
			set { _htmlResult.Value = value; }
		}
		public string DecodedValue
		{
			get { return _htmlResult.DecodedValue; }
		}


		public System.Xml.Linq.XElement XElement
		{
			get { return _htmlResult.XElement; }
		}


		public bool Checked
		{
			get
			{
				return _htmlResult.Checked;
			}
			set
			{
				_htmlResult.Checked = value;
			}
		}

		public int TotalElementsFound
		{
			get { return _htmlResult.TotalElementsFound; }
		}


		public void Click()
		{
			_htmlResult.Click();
		}

		#endregion

		#region IHtmlResult Members


		public bool Disabled
		{
			get { return _htmlResult.Disabled; }
		}

		public string GetAttribute(string attributeName)
		{
			return _htmlResult.GetAttribute(attributeName);
		}

		public void SubmitForm()
		{
			_htmlResult.SubmitForm();
		}


		public IHtmlResult Select(string query)
		{
			return new HtmlResultWrapper(_htmlResult.Select(query));
		}

		#endregion

		#region IEnumerable<IHtmlResult> Members

		public IEnumerator<IHtmlResult> GetEnumerator()
		{
			var innerEnum = _htmlResult.GetEnumerator();
			while (innerEnum.MoveNext())
			{
				yield return new HtmlResultWrapper(innerEnum.Current);
			}
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			var innerEnum = _htmlResult.GetEnumerator();
			while (innerEnum.MoveNext())
			{
				yield return new HtmlResultWrapper(innerEnum.Current);
			}
		}

		#endregion


	}
}

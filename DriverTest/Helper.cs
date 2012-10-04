using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using SimpleBrowser.Network;
using Moq;
using System.Net;
using SimpleBrowser.WebDriver;
using SimpleBrowser;
using System.Text.RegularExpressions;

namespace DriverTest
{
	public class Helper
	{
		internal static string GetFromResources(string resourceName)
		{
			Assembly assem = Assembly.GetExecutingAssembly();
			using (Stream stream = assem.GetManifestResourceStream(resourceName))
			{
				using (var reader = new StreamReader(stream))
				{
					return reader.ReadToEnd();
				}
			}
		}
		/// <summary>
		/// Returns a mocked request instance that will just return OK for any request and have an empty HTML document as its result
		/// </summary>
		/// <returns></returns>
		internal static IWebRequestFactory GetAllways200RequestMocker(IList<Tuple<string, string>> responses = null)
		{
			return new Always200RequestMocker(responses);
		}
		class Always200RequestMocker : IWebRequestFactory
		{
			public Always200RequestMocker(IList<Tuple<string, string>> responses = null)
			{
				if (responses != null)
				{
					foreach (var item in responses)
					{
						_responses.Add(new Tuple<Regex, string>(
							new Regex(item.Item1),
							item.Item2
							));
					}
				}
			}
			private List<Tuple<Regex, string>> _responses = new List<Tuple<Regex, string>>();
			public string ResponseContent(Uri url)
			{
				var tup = _responses.FirstOrDefault(t => t.Item1.IsMatch(url.AbsolutePath));
				return tup==null ?  "" : tup.Item2;
			}
			#region IWebRequestFactory Members

			public IHttpWebRequest GetWebRequest(Uri url)
			{
				var mock = new Mock<IHttpWebRequest>();
				mock.SetupAllProperties();
				mock.Setup(m => m.GetResponse())
					.Returns(() =>
					{
						var mockResponse = new Mock<IHttpWebResponse>();
						mockResponse.SetupAllProperties();
						mockResponse.SetupProperty(m => m.Headers, new WebHeaderCollection());

						byte[] responseContent = Encoding.UTF8.GetBytes(this.ResponseContent(url));
						mockResponse.Setup(r => r.GetResponseStream()).Returns(new MemoryStream(responseContent));
						return mockResponse.Object;
					});
				mock.SetupProperty(m => m.Headers, new WebHeaderCollection());
				mock.Setup(m => m.GetRequestStream()).Returns(new MemoryStream(new byte[2000000]));
				return mock.Object;
			}

			#endregion
		}
		public class BrowserWrapperWithLastRequest : BrowserWrapper
		{
			public BrowserWrapperWithLastRequest(Browser b):base(b)
			{
				b.RequestLogged += new Action<Browser, HttpRequestLog>(HandleRequestLogged);
			}
			public HttpRequestLog LastRequest { get; set; }

			void HandleRequestLogged(Browser b, HttpRequestLog req)
			{
				this.LastRequest = req;
			}

		}

	}
}

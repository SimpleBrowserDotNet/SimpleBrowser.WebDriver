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
		internal static IWebRequestFactory GetAllways200RequestMocker()
		{
			return new Always200RequestMocker();
		}
		class Always200RequestMocker : IWebRequestFactory
		{
			public Always200RequestMocker()
			{
				ResponseContent = "";
			}
			public string ResponseContent { get; set; }
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

						byte[] responseContent = Encoding.UTF8.GetBytes(this.ResponseContent);
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

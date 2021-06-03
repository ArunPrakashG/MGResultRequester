using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MGUResultRequester
{
	public class MGURequester : IDisposable
	{
		private const string URL = "https://117.239.158.204/exQpMgmt/index.php/public/ResultView_ctrl/";
		private readonly HttpClientHandler ClientHandler;
		private readonly HttpClient Client;
		private readonly CookieContainer Cookies = new CookieContainer();
		private bool IsRequestInProgress;

		public MGURequester(IWebProxy proxy = null)
		{
			ClientHandler = new HttpClientHandler()
			{
				AllowAutoRedirect = true,
				CookieContainer = Cookies,
				UseCookies = true,
				Proxy = proxy,
				UseProxy = proxy != null,
				ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
			};

			Client = new HttpClient(ClientHandler, false);
		}

		private async Task WaitWhileRequestInProgress()
		{
			while (IsRequestInProgress)
			{
				await Task.Delay(1);
			}
		}

		public async Task<string> GetResult(string registeredNumber, ExamCode examCode, int maxTries = 100)
		{
			if (string.IsNullOrEmpty(registeredNumber))
			{
				Console.WriteLine("Registered number cant be empty!");
				return null;
			}

			Dictionary<string, string> postData = new Dictionary<string, string>()
			{
				{"prn", registeredNumber },
				{"exam_id", ((int) examCode).ToString() },
				{"btnresult", "Get Result" }
			};

			return await ExecuteRequestAsync(postData, maxTries).ConfigureAwait(false);
		}

		private async Task<string> ExecuteRequestAsync(Dictionary<string, string> postData, int maxTries = 100)
		{
			if (postData == null || postData.Count <= 0)
			{
				return default;
			}

			await WaitWhileRequestInProgress();
			IsRequestInProgress = true;

			try
			{
				for (int i = 0; i < maxTries; i++)
				{
					try
					{
						using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL))
						{
							using (request.Content = new FormUrlEncodedContent(postData))
							{
								using (HttpResponseMessage response = await Client.SendAsync(request).ConfigureAwait(false))
								{
									if (response.StatusCode == HttpStatusCode.GatewayTimeout)
									{
										Console.WriteLine("Request timed out!");
										Console.WriteLine($"Retry count: {i}");
										continue;
									}

									if (!response.IsSuccessStatusCode)
									{
										Debug.WriteLine($"({response.StatusCode}) {response.ReasonPhrase}");
										Console.WriteLine("Request failed.");
										Console.WriteLine($"Retry count: {i}");
										continue;
									}

									return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
								}
							}
						}
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
						Console.WriteLine($"Retry count: {i}");
						continue;
					}
				}

				return null;
			}
			finally
			{
				IsRequestInProgress = false;
			}
		}

		public static async Task<bool> SaveAsHtmlFileAsync(string html, string registeredNumber)
		{
			if (string.IsNullOrEmpty(html) || string.IsNullOrEmpty(registeredNumber))
			{
				return false;
			}

			if (File.Exists(registeredNumber + ".html"))
			{
				File.Delete(registeredNumber + ".html");
			}

			await File.WriteAllTextAsync(registeredNumber + ".html", html).ConfigureAwait(false);
			return File.Exists(registeredNumber + ".html");
		}

		public void Dispose()
		{
			ClientHandler?.Dispose();
			Client?.Dispose();
		}

		public enum ExamCode
		{
			FirstSemCBCSJan2018 = 5,
			SecondSemCBCSMay2018 = 8,
			ThirdSemCBCSOct2018 = 10,
			FirstSemCBCSDec2018 = 13,
			FirstSemCBCSSuppDec2018 = 14,
			FourthSemCBCSMay2019 = 15,
			SecondSemCBCSMay2019 = 17,
			ThirdSemCBCSOct2019 = 22,
			FifthSemCBCSOct2019 = 23,
			FirstSemCBCSOct2019 = 24,
			FifthSemCBCS2017AdmSuppFeb2020 = 27,
			SixthSemCBCSMar2020 = 29,
			FourthSemCBCSMarch2020 = 30,
			SecondSemCBCSOct2020 = 33
		}
	}
}

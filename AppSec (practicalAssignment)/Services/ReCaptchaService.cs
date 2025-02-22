﻿using System.Net;
using System.Text.Json.Nodes;

namespace AppSec__practicalAssignment_.Services
{
	public class ReCaptchaService
	{
		public static async Task<bool> verifyReCaptchaV3(string response, string secret, string verificationUrl)
		{
			using (var client = new HttpClient())
			{
				var content = new MultipartFormDataContent();
				content.Add(new StringContent (WebUtility.HtmlEncode(response)), "response");
				content.Add(new StringContent(secret), "secret");

				var results = await client.PostAsync(verificationUrl, content);

				if (results.IsSuccessStatusCode)
				{
					var strResponse = await results.Content.ReadAsStringAsync();
					Console.WriteLine(strResponse);

					var jsonResponse = JsonNode.Parse(strResponse);
					if (jsonResponse != null)
					{
						var success = ((bool?)jsonResponse["success"]);
						if (success != null && success == true) return true;
					}
				}
			}

			return false;
		}
	}
}

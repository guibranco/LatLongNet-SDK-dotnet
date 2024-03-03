using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LatLongNet;

public static class LatLongNetClient
{
	public static LatLongNetResult Search(string address)
	{
		return SearchAsync(address, CancellationToken.None).Result;
	}

	public static async Task<LatLongNetResult> SearchAsync(string address, CancellationToken token)
	{
		using HttpClient client = new HttpClient();
		client.BaseAddress = new Uri("https://www.latlong.net");
		client.DefaultRequestHeaders.ExpectContinue = false;
		client.DefaultRequestHeaders.TryAddWithoutValidation("x-requested-with", "XMLHttpRequest");
		
		var nameValueCollection = new Dictionary<string, string>
		{
			{ "c1", address },
			{ "action", "gpcm" },
			{ "cp",	string.Empty }
		};
		
		var response = await client.PostAsync("_spm4.php", new FormUrlEncodedContent(nameValueCollection), token).ConfigureAwait(continueOnCapturedContext: false);
		response.EnsureSuccessStatusCode();
		var array = (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)).Split(new char[1] { ',' });
		
		if (array.Length != 2)
		{
			return null;
		}

		return new LatLongNetResult
		{
			Latitude = array[0],
			Longitude = array[1]
		};
	}
}

using System;
using System.Net.Http;

public class Globals
{
	private static string _sessionTicket;

	// replace with your own key and endpoint
	public static string VisionKey = "9c41596b4b2d47f8bc2e5b6f0e6baef9";
	public static string VisionEndpoint = "http://eastus.api.cognitive.microsoft.com/vision/v1.0";

	static Globals()
	{
		HttpClient = new HttpClient();

#if UNITY_EDITOR
		HttpClient.BaseAddress = new Uri("http://localhost:7071/api/");
#else
		HttpClient.BaseAddress = new Uri("https://scavengersrv.azurewebsites.net/api/");
#endif
	}

	public static HttpClient HttpClient { get; set; }

	public static string SessionTicket
	{
		get { return _sessionTicket; }
		set
		{
			_sessionTicket = value;
			HttpClient.DefaultRequestHeaders.Add("SessionTicket", value);
		}
	}

	public static string DisplayName { get; set; }

	public static Item CurrentItem { get; set; }
}

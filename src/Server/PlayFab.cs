using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using PlayFab;
using PlayFab.ServerModels;

namespace ScavengerServer
{
	class PlayFab
    {
		internal static async Task<string> AuthenticateUserAsync(string session, TraceWriter log)
		{
			if(string.IsNullOrEmpty(session))
				return null;

			AuthenticateSessionTicketRequest req = new AuthenticateSessionTicketRequest
			{
				SessionTicket = session
			};

			try
			{
				PlayFabSettings.DeveloperSecretKey = Environment.GetEnvironmentVariable("PlayFabKey");
				PlayFabSettings.TitleId = Environment.GetEnvironmentVariable("PlayFabTitleID");

				var resp = await PlayFabServerAPI.AuthenticateSessionTicketAsync(req);
				return resp.Result.UserInfo.PlayFabId;
			}
			catch(Exception e)
			{
				log.Error($"Error authenticating session {session} --> {e}");
				return null;
			}
		}
    }
}

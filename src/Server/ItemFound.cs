using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ScavengerServer
{
	public static class ItemFound
	{
		[FunctionName("ItemFound")]
		public static async Task<HttpResponseMessage> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "ItemFound/{session}")] FoundItem item,
			string session,
			[DocumentDB("ScavengerDB", "FoundItems", ConnectionStringSetting = "ConnectionString", CreateIfNotExists = true)] IAsyncCollector<FoundItem> document,
			TraceWriter log)
		{
			log.Info("Item found, authenticating user");

			string playFabId = await PlayFab.AuthenticateUserAsync(session, log);

			if(string.IsNullOrEmpty(playFabId))
				return new HttpResponseMessage(HttpStatusCode.BadRequest);

			item.PlayFabId = playFabId;

			await document.AddAsync(item);

			log.Info("Item found complete");
			return new HttpResponseMessage(HttpStatusCode.OK);
		}
	}
}

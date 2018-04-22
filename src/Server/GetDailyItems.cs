using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ScavengerServer
{
	public static class GetDailyItems
	{
		[FunctionName("GetDailyItems")]
		public static async Task<HttpResponseMessage> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route="GetDailyItems/{session}")]HttpRequestMessage req,
			string session,
			[DocumentDB("ScavengerDB", "Items", ConnectionStringSetting = "ConnectionString")] DocumentClient client,
			TraceWriter log)
		{
			log.Info($"GetDailyItems begin");

			string playFabId = await PlayFab.AuthenticateUserAsync(session, log);

			if(string.IsNullOrEmpty(playFabId))
				return new HttpResponseMessage(HttpStatusCode.BadRequest);

			Uri collectionUri = UriFactory.CreateDocumentCollectionUri("ScavengerDB", "Items");
			IOrderedQueryable<DailyItems> query = client.CreateDocumentQuery<DailyItems>(collectionUri);
			IQueryable<DailyItems> documents =
				from di in query 
				where di.Date == DateTime.Now.ToString("yyyy-MM-dd")
				select di;

			log.Info($"GetDailyItems complete");

			return req.CreateResponse(HttpStatusCode.OK, documents);
		}
	}
}

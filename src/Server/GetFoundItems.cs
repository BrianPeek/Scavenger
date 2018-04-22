using System;
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
	public static class GetFoundItems
	{
		[FunctionName("GetFoundItems")]
		public static async Task<HttpResponseMessage> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route="GetFoundItems/{session}/{date}")]HttpRequestMessage req,
			string session,
			string date,
			[DocumentDB("ScavengerDB", "FoundItems", ConnectionStringSetting = "ConnectionString")] DocumentClient client,
			TraceWriter log)
		{
			log.Info($"GetFoundItems for date: {date}");

			string playFabId = await PlayFab.AuthenticateUserAsync(session, log);

			if(string.IsNullOrEmpty(playFabId))
				return new HttpResponseMessage(HttpStatusCode.BadRequest);

			Uri collectionUri = UriFactory.CreateDocumentCollectionUri("ScavengerDB", "FoundItems");
			IOrderedQueryable<FoundItem> query = client.CreateDocumentQuery<FoundItem>(collectionUri);
			IQueryable<FoundItem> documents =
				from fi in query 
				where fi.Date == date && fi.PlayFabId == playFabId
				select fi;

			log.Info($"GetFoundItems complete");

			return req.CreateResponse(HttpStatusCode.OK, documents);
		}
	}
}

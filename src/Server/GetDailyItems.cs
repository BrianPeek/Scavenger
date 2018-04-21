using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ScavengerServer
{
	public static class GetDailyItems
	{
		[FunctionName("GetDailyItems")]
		public static async Task<HttpResponseMessage> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route="GetDailyItems/{session}/{date}")]HttpRequestMessage req,
			string session,
			string date,
			[DocumentDB("ScavengerDB", "Items", ConnectionStringSetting = "ConnectionString", SqlQuery = "SELECT * from i where i.date={date}")] IEnumerable<DailyItems> documents,
			TraceWriter log)
		{
			log.Info($"GetDailyItems for date: {date}");

			string playFabId = await PlayFab.AuthenticateUserAsync(session, log);

			if(string.IsNullOrEmpty(playFabId))
				return new HttpResponseMessage(HttpStatusCode.BadRequest);

			log.Info($"GetDailyItems complete");
			return req.CreateResponse(HttpStatusCode.OK, documents);
		}
	}
}

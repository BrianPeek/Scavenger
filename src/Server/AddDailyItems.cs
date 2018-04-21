using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ScavengerServer
{
	public static class AddDailyItems
	{
		[FunctionName("AddDailyItems")]
		public static HttpResponseMessage Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = null)] DailyItems items,
			[DocumentDB("ScavengerDB", "Items", ConnectionStringSetting = "ConnectionString")] out DailyItems document,
			TraceWriter log)
		{
			log.Info("AddDailyItems");

			// TODO: Authenticate admin

			document = items;

			return new HttpResponseMessage(HttpStatusCode.OK);
		}
	}
}

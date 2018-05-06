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
			[HttpTrigger(AuthorizationLevel.Function, "POST", Route = null)] DailyItems items,
			[DocumentDB(Constants.CosmosDbName, Constants.ItemsCollectionName, ConnectionStringSetting = Constants.ConnectionStringSettingName)] out DailyItems document,
			TraceWriter log)
		{
			log.Info("AddDailyItems");

			// TODO: Authenticate admin

			try
			{
				document = items;

				return new HttpResponseMessage(HttpStatusCode.OK);
			}
			catch (System.Exception ex)
			{
				document = null;
				log.Error(ex.Message);
				return new HttpResponseMessage(HttpStatusCode.InternalServerError);
			}
		}
	}
}

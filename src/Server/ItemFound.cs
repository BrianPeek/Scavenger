using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using PlayFab;
using PlayFab.ServerModels;

namespace ScavengerServer
{
	public static class ItemFound
	{
		[FunctionName("ItemFound")]
		public static async Task<HttpResponseMessage> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "ItemFound")] Item item,
			HttpRequestMessage req,
			[DocumentDB(Constants.CosmosDbName, Constants.FoundItemsCollectionName, ConnectionStringSetting = Constants.ConnectionStringSettingName, CreateIfNotExists = true)] IAsyncCollector<FoundItem> document,
			TraceWriter log)
		{
			log.Info("ItemFound, authenticating user");

			try
			{
				string playFabId = await PlayFab.AuthenticateUserAsync(req, log);

				if (string.IsNullOrEmpty(playFabId))
					return new HttpResponseMessage(HttpStatusCode.Unauthorized);

				log.Info("Adding found item");

				item.Found = true;
				FoundItem fi = new FoundItem
				{
					Date = DateTime.Now.ToString(Constants.DateFormat),
					PlayFabId = playFabId,
					Item = item
				};
				await document.AddAsync(fi);

				log.Info("Updating leaderboard");
				UpdatePlayerStatisticsRequest playerStatReq = new UpdatePlayerStatisticsRequest
				{
					PlayFabId = playFabId,
					Statistics = new List<StatisticUpdate>
					{
						new StatisticUpdate
						{
							StatisticName = "Score",
							Value = 1
						}
					}
				};
				PlayFabResult<UpdatePlayerStatisticsResult> result = await PlayFabServerAPI.UpdatePlayerStatisticsAsync(playerStatReq);

				if (result.Error != null)
					log.Error($"Error updating leaderboard: {result.Error.GenerateErrorReport()}");
				else
					log.Info($"PlayFab complete: {result.Result}");

				log.Info("ItemFound complete");
				return new HttpResponseMessage(HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				log.Error(ex.Message);
				return new HttpResponseMessage(HttpStatusCode.InternalServerError);
			}
		}
	}
}

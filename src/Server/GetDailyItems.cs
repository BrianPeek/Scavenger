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
	public static class GetDailyItems
	{
		[FunctionName("GetDailyItems")]
		public static async Task<HttpResponseMessage> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route="GetDailyItems")] HttpRequestMessage req,
			[DocumentDB(Constants.CosmosDbName, Constants.ItemsCollectionName, ConnectionStringSetting = Constants.ConnectionStringSettingName)] DocumentClient client,
			TraceWriter log)
		{
			log.Info($"GetDailyItems begin");

			try
			{
				string playFabId = await PlayFab.AuthenticateUserAsync(req, log);
				string date = DateTime.Now.ToString(Constants.DateFormat);

				if (string.IsNullOrEmpty(playFabId))
					return new HttpResponseMessage(HttpStatusCode.Unauthorized);

				// Get the list of items for today
				Uri collectionUri = UriFactory.CreateDocumentCollectionUri(Constants.CosmosDbName, Constants.ItemsCollectionName);
				IOrderedQueryable<DailyItems> query = client.CreateDocumentQuery<DailyItems>(collectionUri);
				IQueryable<DailyItems> dailyItemsDoc =
					from di in query
					where di.Date == date
					select di;
				DailyItems[] dailyItemsArray = dailyItemsDoc.ToArray();
				if(dailyItemsArray.Length == 0)
				{
					log.Error($"No daily items found for {date}");
					return req.CreateErrorResponse(HttpStatusCode.InternalServerError, $"No daily items found for {date}");
				}

				// and pull back just today's items
				DailyItems dailyItems = dailyItemsArray.Single();

				// get all items a user has found for today
				Uri foundUri = UriFactory.CreateDocumentCollectionUri(Constants.CosmosDbName, Constants.FoundItemsCollectionName);
				IOrderedQueryable<FoundItem> foundQuery = client.CreateDocumentQuery<FoundItem>(foundUri);
				IQueryable<FoundItem> foundItemsDoc =
					from found in foundQuery
					where found.PlayFabId == playFabId && found.Date == date
					select found;

				FoundItem[] foundItems = foundItemsDoc.ToArray();

				// mark all found items by enumerating through both arrays
				foreach(Item i in dailyItems.Items)
				{
					foreach(FoundItem fi in foundItems)
					{
						if(fi.Item.Id == i.Id)
						{
							i.Found = true;
							break;
						}
					}
				}

				log.Info($"GetDailyItems complete");

				return req.CreateResponse(HttpStatusCode.OK, dailyItems);
			}
			catch (Exception ex)
			{
				log.Error(ex.Message);
				return req.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
			}
		}
	}
}

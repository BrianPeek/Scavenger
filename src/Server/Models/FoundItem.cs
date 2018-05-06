using Newtonsoft.Json;

namespace ScavengerServer
{
	public class FoundItem
	{
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "date")]
		public string Date { get; set; }

		[JsonProperty(PropertyName = "playfabid")]
		public string PlayFabId { get; set; }

		[JsonProperty(PropertyName = "item")]
		public Item Item { get; set; }
	}
}
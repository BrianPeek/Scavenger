using System.Collections.Generic;
using Newtonsoft.Json;

namespace ScavengerServer
{
	public class DailyItems
	{
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "date")]
		public string Date { get; set; }

		[JsonProperty(PropertyName = "items")]
		public List<Item> Items { get; set; }
	}


	public class Item
	{
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "tags")]
		public List<string> Tags { get; set; }
	}

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
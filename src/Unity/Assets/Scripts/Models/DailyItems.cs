using System.Collections.Generic;
using Newtonsoft.Json;

public class DailyItems
{
	[JsonProperty(PropertyName = "id")]
	public string Id { get; set; }

	[JsonProperty(PropertyName = "date")]
	public string Date { get; set; }

	[JsonProperty(PropertyName = "items")]
	public List<Item> Items { get; set; }
}

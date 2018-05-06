using System.Collections.Generic;
using Newtonsoft.Json;

namespace ScavengerServer
{
	public class Item
	{
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "tags")]
		public List<string> Tags { get; set; }

		[JsonProperty(PropertyName = "found")]
		public bool Found { get; set; }
	}
}
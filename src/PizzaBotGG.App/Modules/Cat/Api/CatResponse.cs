using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PizzaBotGG.App.Modules.Cat.Api
{
	public class CatResponse
	{
		[JsonPropertyName("url")]
		public string Url { get; set; }

		[JsonPropertyName("breeds")]
		public List<BreedResponse> Breeds { get; set; }
	}
}

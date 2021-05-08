using System.Text.Json.Serialization;

namespace PizzaBotGG.App.Modules.Cat.Api
{
	public class BreedResponse
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}

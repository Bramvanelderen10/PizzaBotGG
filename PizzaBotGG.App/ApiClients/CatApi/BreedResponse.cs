using System.Text.Json.Serialization;

namespace PizzaBotGG.App.ApiClients.CatApi
{
	public class BreedResponse
	{
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}

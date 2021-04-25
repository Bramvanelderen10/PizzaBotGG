using System.Text.Json.Serialization;

namespace PizzaBotGG.App.ApiClients.CatApi
{
	public class CatResponse
	{
		[JsonPropertyName("url")]
		public string Url { get; set; }
	}
}

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Image
{
	[Group("cat")]
	public class CatModule : BaseCommandModule
	{
		[GroupCommand]
		
		public async Task CatCommand(CommandContext context)
		{
			var httpClient = new HttpClient();
			var response = await httpClient.GetAsync("https://api.thecatapi.com/v1/images/search?format=json");
			var imageContent = await response.Content.ReadAsStringAsync();
			var catResponses = JsonSerializer.Deserialize<List<CatApiResponse>>(imageContent);

			var builder = new DiscordEmbedBuilder();
			builder.ImageUrl = catResponses.First().Url;
			var embedded = builder.Build();
			await context.RespondAsync(embedded);
		}

		[Command("breeds")]

		public async Task BreedsCommand(CommandContext context)
		{
			
		}

		private class CatApiResponse
		{
			[JsonPropertyName("url")]
			public string Url { get; set; }
		}
	}
}

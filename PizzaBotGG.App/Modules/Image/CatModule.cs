using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using PizzaBotGG.App.ApiClients.CatApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Image
{
	[Group("cat")]
	public class CatModule : BaseCommandModule
	{
		private readonly ICatApi _catApi;

		public CatModule(ICatApi catApi)
		{
			_catApi = catApi;
		}

		[GroupCommand]
		
		public async Task CatCommand(CommandContext context, string breed = null)
		{
			var breedId = await GetBreedId(breed);
			var catResponses = await _catApi.SearchCats(breedId: breedId);

			if (!catResponses.Any())
			{
				await context.RespondAsync("No cats found :(");
				return;
			}

			var builder = new DiscordEmbedBuilder();
			builder.ImageUrl = catResponses.First().Url;
			var embedded = builder.Build();
			await context.RespondAsync(embedded);
		}

		private async Task<string> GetBreedId(string breedName)
		{
			if (string.IsNullOrWhiteSpace(breedName)) return null;

			var breeds = await _catApi.SearchBreeds(breedName);
			var breed = breeds.FirstOrDefault();

			return breed.Id;
		}

		[Command("breeds")]

		public async Task BreedsCommand(CommandContext context, string breedName = null)
		{
			var breedResponses = await GetBreeds(breedName);
			var breedNames = breedResponses.Select(x => x.Name).ToList();
			var responseText = string.Join(Environment.NewLine, breedNames);

			await context.RespondAsync(responseText);
		}

		private async Task<List<BreedResponse>> GetBreeds(string breedName)
		{
			if (breedName == null)
			{
				return await _catApi.GetBreeds(3);
			}
			else
			{
				return await _catApi.SearchBreeds(breedName);
			}
		}
	}
}

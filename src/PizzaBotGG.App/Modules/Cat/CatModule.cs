using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MimeTypes;
using PizzaBotGG.App.Modules.Cat.Api;
using System;
using System.Collections.Generic;
using System.Linq;
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
		public async Task CatImageCommand(CommandContext context, string breed = null)
		{
			var mimeTypes = new[]
			{
				MimeTypeMap.GetMimeType("jpeg"),
				MimeTypeMap.GetMimeType("png"),
			};

			await AddCatToContext(context, mimeTypes, breed);
		}

		private async Task<string> GetBreedId(string breedName)
		{
			if (string.IsNullOrWhiteSpace(breedName)) return null;

			var breeds = await _catApi.SearchBreeds(breedName);
			var breed = breeds.FirstOrDefault();

			return breed?.Id;
		}

		[Command("breeds")]
		public async Task BreedsCommand(CommandContext context, string breedName = null)
		{
			var breedResponses = await GetBreeds(breedName);
			var breedNames = breedResponses.Select(x => x.Name).ToList();
			var responseText = string.Join(Environment.NewLine, breedNames);

			await context.RespondAsync(responseText);
		}

		[Command("gif")]
		public async Task CatGifCommand(CommandContext context, string breedName = null)
		{
			var mimeType = MimeTypeMap.GetMimeType("gif");
			await AddCatToContext(context, new[] { mimeType }, breedName);
		}


		private async Task AddCatToContext(
			CommandContext context,
			string[] mimeTypes,
			string breed = null)
		{
			string breedId = null;
			if (!string.IsNullOrWhiteSpace(breed))
			{
				breedId = await GetBreedId(breed);
				if (breedId == null)
				{
					await context.RespondAsync("Cat breed not found :(");
					return;
				}
			}

			var catResponses = await _catApi.SearchCats(breedId: breedId, mimeTypes: mimeTypes);
			if (!catResponses.Any())
			{
				await context.RespondAsync("No cats found :(");
				return;
			}

			var catResponse = catResponses.First();
			var catBreed = catResponse.Breeds.FirstOrDefault();

			var builder = new DiscordEmbedBuilder();
			builder.ImageUrl = catResponses.First().Url;
			var embedded = builder.Build();

			if (catBreed == null)
			{
				await context.RespondAsync(embedded);
			}
			else
			{
				await context.RespondAsync($"This is a {catBreed.Name}", embedded);
			}
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

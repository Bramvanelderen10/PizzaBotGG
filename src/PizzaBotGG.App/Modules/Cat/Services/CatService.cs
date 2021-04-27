using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using PizzaBotGG.App.ExceptionHandling;
using PizzaBotGG.App.Modules.Cat.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Cat.Services
{
	public class CatService : ICatService
	{
		private readonly ICatApi _catApi;

		public CatService(ICatApi catApi)
		{
			_catApi = catApi;
		}

		public async Task<DiscordEmbed> GetCatEmbed(
			string[] mimeTypes,
			string breed = null)
		{
			var breedId = await GetBreedId(breed);
			var catResponses = await _catApi.SearchCats(breedId: breedId, mimeTypes: mimeTypes);

			if (!catResponses.Any()) throw new CommandException("No cats found :(");

			var catResponse = catResponses.First();
			var catBreed = catResponse.Breeds.FirstOrDefault();

			var builder = new DiscordEmbedBuilder();
			builder.ImageUrl = catResponse.Url;
			builder.Title = catBreed != null ? $"This is a {catBreed.Name}" : null;
			var embed = builder.Build();

			return embed;
		}

		public async Task<string> GetBreedsResponse(string breedName)
		{
			var breedResponses = breedName == null ? await _catApi.GetBreeds(999) : await _catApi.SearchBreeds(breedName);
			var breedNames = breedResponses.Select(x => x.Name).ToList();
			var responseText = string.Join(Environment.NewLine, breedNames);
			return responseText;
		}

		private async Task<string> GetBreedId(string breedName)
		{
			if (string.IsNullOrWhiteSpace(breedName)) return null;

			var breeds = await _catApi.SearchBreeds(breedName);
			var breed = breeds.FirstOrDefault();

			if (breed == null) throw new CommandException("Breed not found");

			return breed?.Id;
		}


	}
}

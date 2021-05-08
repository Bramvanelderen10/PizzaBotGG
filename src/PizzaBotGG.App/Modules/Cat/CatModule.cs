using DSharpPlus.Entities;
using MimeTypes;
using PizzaBotGG.App.DiscordSlashCommandModule;
using PizzaBotGG.App.DiscordSlashCommandModule.Attributes;
using PizzaBotGG.App.Modules.Cat.Services;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Image
{
	[SlashCommandGroup("cat", "cat module")]
	public class CatModule : SlashModule
	{
		private readonly ICatService _catService;

		public CatModule(ICatService catService)
		{
			_catService = catService;
		}

		[SlashCommand("random", "gets a random cat image")]
		public async Task<DiscordEmbed> CatImageCommand(string breedName = null)
		{
			var mimeTypes = new[]
			{
				MimeTypeMap.GetMimeType("jpeg"),
				MimeTypeMap.GetMimeType("png"),
			};

			var catEmbed = await _catService.GetCatEmbed(mimeTypes, breedName);
			return catEmbed;
		}

		[SlashCommand("breeds", "gets all breeds")]
		public async Task<string> BreedsCommand(string breedName = null)
		{
			var breedsResponse = await _catService.GetBreedsResponse(breedName);
			return breedsResponse;
		}

		[SlashCommand("gif", "gets random cat gif")]
		public async Task<DiscordEmbed> CatGifCommand(string breedName = null)
		{
			var mimeTypes = new[]
			{
				MimeTypeMap.GetMimeType("gif"),
			};

			var catEmbed = await _catService.GetCatEmbed(mimeTypes, breedName);
			return catEmbed;
		}


	}
}

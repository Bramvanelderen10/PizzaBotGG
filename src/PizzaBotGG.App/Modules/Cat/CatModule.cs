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

		[SlashCommand("random", "Random cat")]
		public async Task<DiscordEmbed> CatImageCommand(string breed = null)
		{
			var mimeTypes = new[]
			{
				MimeTypeMap.GetMimeType("jpeg"),
				MimeTypeMap.GetMimeType("png"),
			};

			var catEmbed = await _catService.GetCatEmbed(mimeTypes, breed);
			return catEmbed;
		}

		[SlashCommand("breeds", "Get all breeds")]
		public async Task<string> BreedsCommand(string breed = null)
		{
			var breedsResponse = await _catService.GetBreedsResponse(breed);
			return breedsResponse;
		}

		[SlashCommand("gif", "Get cat gif")]
		public async Task<DiscordEmbed> CatGifCommand(string breed = null)
		{
			var mimeTypes = new[]
			{
				MimeTypeMap.GetMimeType("gif"),
			};

			var catEmbed = await _catService.GetCatEmbed(mimeTypes, breed);
			return catEmbed;
		}


	}
}

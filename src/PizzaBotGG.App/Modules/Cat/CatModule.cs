using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MimeTypes;
using PizzaBotGG.App.Modules.Cat.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Image
{
	[Group("cat")]
	[Aliases("c")]
	public class CatModule : BaseCommandModule
	{
		private readonly ICatService _catService;

		public CatModule(ICatService catService)
		{
			_catService = catService;
		}

		[GroupCommand]
		public async Task CatImageCommand(CommandContext context, string breedName = null)
		{
			var mimeTypes = new[]
			{
				MimeTypeMap.GetMimeType("jpeg"),
				MimeTypeMap.GetMimeType("png"),
			};

			var catEmbed = await _catService.GetCatEmbed(mimeTypes, breedName);
			await context.RespondAsync(catEmbed);
		}

		[Command("breeds")]
		public async Task BreedsCommand(CommandContext context, string breedName = null)
		{
			var breedsResponse = await _catService.GetBreedsResponse(breedName);
			await context.RespondAsync(breedsResponse);
		}

		[Command("gif")]
		public async Task CatGifCommand(CommandContext context, string breedName = null)
		{
			var mimeTypes = new[]
			{
				MimeTypeMap.GetMimeType("gif"),
			};

			var catEmbed = await _catService.GetCatEmbed(mimeTypes, breedName);
			await context.RespondAsync(catEmbed);
		}

		
	}
}

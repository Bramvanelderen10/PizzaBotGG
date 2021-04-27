using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using PizzaBotGG.App.Modules.Waifu.Enums;
using PizzaBotGG.App.Modules.Waifu.Services;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Waifu
{
	[Group("waifu")]
	[Aliases("w")]
	public class WaifuModule : BaseCommandModule
	{
		private readonly IWaifuService _waifuService;

		public WaifuModule(IWaifuService waifuService)
		{
			_waifuService = waifuService;
		}

		[GroupCommand]
		public async Task WaifuImageCommand(CommandContext context, WaifuSFWCategory? category = null)
		{
			var embed = await _waifuService.GetSFWWaifuEmbed(category);
			await context.RespondAsync(embed);
		}

		[Command("nsfw")]
		public async Task WaifuNSFWImageCommand(CommandContext context, WaifuNSFWCategory? category = null)
		{
			var embed = await _waifuService.GetNSFWWaifuEmbed(category);
			await context.RespondAsync(embed);
		}
	}
}

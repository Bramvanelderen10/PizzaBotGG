using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using PizzaBotGG.App.DiscordSlashCommandModule;
using PizzaBotGG.App.DiscordSlashCommandModule.Attributes;
using PizzaBotGG.App.Modules.Waifu.Enums;
using PizzaBotGG.App.Modules.Waifu.Services;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Waifu
{
	[SlashCommandGroup("waifu", "Waifu image commands")]
	public class WaifuModule : SlashModule
	{
		private readonly IWaifuService _waifuService;

		public WaifuModule(IWaifuService waifuService)
		{
			_waifuService = waifuService;
		}

		[SlashCommand("random", "A random waifu image")]
		public async Task<DiscordEmbed> WaifuImageCommand(WaifuSFWCategory? category = null)
		{
			var embed = await _waifuService.GetSFWWaifuEmbed(category);
			return embed;
		}

		[SlashCommand("nsfw", "adult images to keep kamal happy")]
		public async Task<DiscordEmbed> WaifuNSFWImageCommand(WaifuNSFWCategory? category = null)
		{
			var embed = await _waifuService.GetNSFWWaifuEmbed(category);

			return embed;
		}
	}
}

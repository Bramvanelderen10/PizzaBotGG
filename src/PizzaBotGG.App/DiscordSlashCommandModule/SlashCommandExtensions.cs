using DSharpPlus;
using System;
using System.Threading.Tasks;

namespace PizzaBotGG.App.DiscordSlashCommandModule
{
	public static class SlashCommandExtensions
	{
		public static async Task<SlashCommandService> AddSlashCommands(this DiscordClient client, Action<SlashCommandConfiguration> configureOptions = null)
		{
			var configuration = new SlashCommandConfiguration();
			if (configureOptions != null) configureOptions(configuration);

			var slashCommandService = new SlashCommandService(client, configuration);
			await slashCommandService.RegisterSlashCommands();

			return slashCommandService;
		}
	}
}

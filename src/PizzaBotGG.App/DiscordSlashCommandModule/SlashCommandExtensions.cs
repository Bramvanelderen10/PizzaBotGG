using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBotGG.App.DiscordSlashCommandModule
{
	public static class SlashCommandExtensions
	{
		public static async Task<SlashCommandService> AddSlashCommands(this DiscordClient client)
		{
			var slashCommandService = new SlashCommandService(client);

			await slashCommandService.RegisterSlashCommands();
			slashCommandService.HandleSlashCommands();

			return slashCommandService;
		}
	}
}

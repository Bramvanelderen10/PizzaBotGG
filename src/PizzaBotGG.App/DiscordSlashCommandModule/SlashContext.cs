using DSharpPlus;
using DSharpPlus.Entities;
using PizzaBotGG.App.DiscordSlashCommandModule.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaBotGG.App.DiscordSlashCommandModule
{
	public class SlashContext
	{
		public SlashContext(DiscordInteraction interaction, List<BaseSlashCommand> baseSlashCommands, IServiceProvider serviceProvider, DiscordClient client)
		{
			Interaction = interaction;
			BaseSlashCommands = baseSlashCommands;
			ServiceProvider = serviceProvider;
			Client = client;
		}

		public DiscordInteraction Interaction { get; }
		public List<BaseSlashCommand> BaseSlashCommands { get; }
		public IServiceProvider ServiceProvider { get; }
		public DiscordClient Client { get; }

		public async Task RespondAsync(string message)
		{
			var builder = new DiscordWebhookBuilder();
			builder.WithContent(message);
			await Interaction.EditOriginalResponseAsync(builder);
		}

		public async Task RespondAsync(DiscordEmbed discordEmbed)
		{
			var builder = new DiscordWebhookBuilder();
			builder.AddEmbed(discordEmbed);
			await Interaction.EditOriginalResponseAsync(builder);
		}
	}
}

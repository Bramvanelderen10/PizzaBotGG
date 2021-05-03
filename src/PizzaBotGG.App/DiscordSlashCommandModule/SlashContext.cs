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
			var webhookBuilder = new DiscordWebhookBuilder();
			webhookBuilder.WithContent(message);
			await Interaction.EditOriginalResponseAsync(webhookBuilder);
		}

		public async Task RespondAsync(DiscordEmbed discordEmbed)
		{
			var webhookBuilder = new DiscordWebhookBuilder();
			webhookBuilder.AddEmbed(discordEmbed);
			await Interaction.EditOriginalResponseAsync(webhookBuilder);
		}
	}
}

using DSharpPlus.Entities;
using PizzaBotGG.App.DiscordSlashCommandModule.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaBotGG.App.DiscordSlashCommandModule
{
	public class SlashContext
	{
		public SlashContext(DiscordInteraction interaction, List<BaseSlashCommand> baseSlashCommands, IServiceProvider serviceProvider)
		{
			Interaction = interaction;
			BaseSlashCommands = baseSlashCommands;
			ServiceProvider = serviceProvider;
		}

		public DiscordInteraction Interaction { get; }
		public List<BaseSlashCommand> BaseSlashCommands { get; }
		public IServiceProvider ServiceProvider { get; }

		public async Task Respond(string message)
		{
			var webhookBuilder = new DiscordWebhookBuilder();
			webhookBuilder.WithContent(message);
			await Interaction.EditOriginalResponseAsync(webhookBuilder);
		}

		public async Task Respond(DiscordEmbed discordEmbed)
		{
			var webhookBuilder = new DiscordWebhookBuilder();
			webhookBuilder.AddEmbed(discordEmbed);
			await Interaction.EditOriginalResponseAsync(webhookBuilder);
		}
	}
}

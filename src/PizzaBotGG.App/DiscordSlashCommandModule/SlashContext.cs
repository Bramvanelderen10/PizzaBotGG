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
			var interactionBuilder = new DiscordInteractionResponseBuilder();
			interactionBuilder.WithContent(message);
			await Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, interactionBuilder);
		}

		public async Task Respond(DiscordEmbed discordEmbed)
		{
			var interactionBuilder = new DiscordInteractionResponseBuilder();
			interactionBuilder.AddEmbed(discordEmbed);
			await Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.ChannelMessageWithSource, interactionBuilder);
		}
	}
}

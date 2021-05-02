using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using PizzaBotGG.App.DiscordSlashCommandModule.Models;
using PizzaBotGG.App.DiscordSlashCommandModule.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PizzaBotGG.App.DiscordSlashCommandModule
{
	public class SlashCommandService
	{
		private readonly DiscordClient _client;
		private readonly List<BaseSlashCommand> _baseSlashCommands;

		public SlashCommandService(
			DiscordClient client)
		{
			_client = client;
			_baseSlashCommands = BuildBaseSlashCommands();
		}

		private List<BaseSlashCommand> BuildBaseSlashCommands()
		{
			var moduleTypes = ModuleTypeLoader.GetModuleTypes();
			var baseSlashCommands = moduleTypes
				.SelectMany(moduleType => SlashCommandsLoader.GetBaseSlashCommandsForModule(moduleType))
				.ToList();

			return baseSlashCommands;
		}
		

		public async Task RegisterSlashCommands()
		{
			await _client.InitializeAsync();
			var applicationCommands = ApplicationCommandLoader.GetApplicationCommands(_baseSlashCommands);

			await _client.BulkOverwriteGuildApplicationCommandsAsync(772864356504698912, applicationCommands);
		}		

		public void HandleSlashCommands()
		{
			_client.InteractionCreated += OnInteractionCreated;
		}

		private Task OnInteractionCreated(DiscordClient sender, DSharpPlus.EventArgs.InteractionCreateEventArgs e)
		{

			var interaction = e.Interaction;
			var data = interaction.Data;

			



			throw new System.NotImplementedException();
		}
	}
}

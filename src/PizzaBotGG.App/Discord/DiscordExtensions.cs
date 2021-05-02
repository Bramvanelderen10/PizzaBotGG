using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Discord
{
	public static class DiscordExtensions
	{
		public static async Task RegisterSlashCommands(this CommandsNextExtension commandsNext)
		{
			throw new NotImplementedException("EXPIRIMENTAL CODE BELOW");
			var client = commandsNext.Client;
			await client.InitializeAsync();
			var commands = commandsNext.RegisteredCommands;

			var createCommandTasks = new List<Task>();
			foreach(var commandPair in commands)
			{
				var key = commandPair.Key;
				var command = commandPair.Value;

				//If alias then skip skip skip
				if (key != command.Name) continue;

				//Help is a default dsharp command, which we ignore
				if (key == "help") continue;
								
				var applicationCommand = new DiscordApplicationCommand(command.Name, "test");
				//var createCommandTask = client.CreateGlobalApplicationCommandAsync(applicationCommand);
				var createCommandTask = client.CreateGuildApplicationCommandAsync(772864356504698912, applicationCommand);
				createCommandTasks.Add(createCommandTask);
			}

			await Task.WhenAll(createCommandTasks);
		}
	}
}

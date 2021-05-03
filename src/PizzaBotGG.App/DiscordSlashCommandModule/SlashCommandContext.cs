using System.Reflection;
using PizzaBotGG.App.DiscordSlashCommandModule.Models;

namespace PizzaBotGG.App.DiscordSlashCommandModule
{
	public class SlashCommandContext
	{
		public SlashCommandContext(SlashCommand command, object[] parameters)
		{
			Command = command;
			Parameters = parameters;
		}

		public SlashCommand Command { get; }
		public object[] Parameters { get; }
	}
}

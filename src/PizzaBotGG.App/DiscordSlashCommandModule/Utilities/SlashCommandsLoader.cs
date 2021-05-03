using PizzaBotGG.App.DiscordSlashCommandModule.Attributes;
using PizzaBotGG.App.DiscordSlashCommandModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Utilities
{
	public class SlashCommandsLoader
	{
		public static List<BaseSlashCommand> GetBaseSlashCommandsForModule(Type moduleType)
		{
			var slashCommands = GetSlashCommandsForType(moduleType);

			var groupAttribute = moduleType.GetCustomAttribute<SlashCommandGroupAttribute>();

			//If no group then return the slash commands directly
			if (groupAttribute == null) return slashCommands.OfType<BaseSlashCommand>().ToList();

			//If group process group attribute then put slash commands as children
			var slashCommandGroup = new SlashCommandGroup(groupAttribute.Name, groupAttribute.Description, moduleType, slashCommands);

			return new List<BaseSlashCommand> { slashCommandGroup };
		}

		private static List<SlashCommand> GetSlashCommandsForType(Type commandType)
		{
			var methodInfoList = commandType.GetMethods();
			var slashCommands = methodInfoList.Select(methodInfo =>
				{
					var commandAttribute = methodInfo.GetCustomAttribute<SlashCommandAttribute>();
					if (commandAttribute == null) return null;

					var isNsfwCommand = methodInfo.GetCustomAttribute<SlashRequiresNsfwAttribute>() is SlashRequiresNsfwAttribute;
					var parameters = methodInfo.GetParameters();

					var slashCommandParameters = parameters
						.Select(parameter => new SlashCommandParameter(parameter.Name, parameter.ParameterType, parameter.IsOptional))
						.ToArray();

					return new SlashCommand(commandAttribute.Name, commandAttribute.Description, commandType, methodInfo, isNsfwCommand, slashCommandParameters);
				})
				.Where(slashCommand => slashCommand != null)
				.ToList();

			return slashCommands;
		}
	}
}

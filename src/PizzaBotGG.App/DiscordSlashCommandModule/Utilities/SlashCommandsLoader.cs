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
			
			//Get aliases for groups
			var aliasAttributes = moduleType.GetCustomAttributes<SlashAliasAttribute>();
			var slashCommandGroupAliasses = aliasAttributes
				.Select(aliasAttribute => new SlashCommandGroup(aliasAttribute.Alias, groupAttribute.Description, moduleType, slashCommands))
				.ToList();

			//Build command array
			var baseSlashCommands = new List<BaseSlashCommand> { slashCommandGroup };
			baseSlashCommands.AddRange(slashCommandGroupAliasses);

			var rootCommands = slashCommands
				.Where(x => x.DuplicateCommandToRoot)
				.ToList();
				
			baseSlashCommands.AddRange(rootCommands);

			return baseSlashCommands;
		}

		private static List<SlashCommand> GetSlashCommandsForType(Type commandType)
		{
			var methodInfoList = commandType.GetMethods();
			var slashCommands = methodInfoList.SelectMany(methodInfo =>
				{
					var commandAttribute = methodInfo.GetCustomAttribute<SlashCommandAttribute>();
					if (commandAttribute == null) return new List<SlashCommand>();

					var isNsfwCommand = methodInfo.GetCustomAttribute<SlashRequiresNsfwAttribute>() is SlashRequiresNsfwAttribute;
					var parameters = methodInfo.GetParameters();

					var slashCommandParameters = parameters
						.Select(parameter => new SlashCommandParameter(parameter.Name, parameter.ParameterType, parameter.IsOptional))
						.ToArray();

					var mainCommand = new SlashCommand(commandAttribute.Name, commandAttribute.Description, commandType, methodInfo, isNsfwCommand, commandAttribute.DuplicateCommandToRoot, slashCommandParameters);
					var commands = new List<SlashCommand>{ mainCommand };

					var aliasAttributes = methodInfo.GetCustomAttributes<SlashAliasAttribute>();
					var aliasCommands = aliasAttributes.Select(aliasAttribute => {
						return new SlashCommand(aliasAttribute.Alias, commandAttribute.Description, commandType, methodInfo, isNsfwCommand, commandAttribute.DuplicateCommandToRoot, slashCommandParameters);
					}).ToList();

					commands.AddRange(aliasCommands);

					return commands;
				})
				.Where(slashCommand => slashCommand != null)
				.ToList();

			return slashCommands;
		}
	}
}

using DSharpPlus;
using DSharpPlus.Entities;
using PizzaBotGG.App.DiscordSlashCommandModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Utilities
{
	public static class ApplicationCommandLoader
	{
		public static List<DiscordApplicationCommand> GetApplicationCommands(List<BaseSlashCommand> baseSlashCommands)
		{
			var rootSlashCommands = baseSlashCommands.OfType<SlashCommand>().ToList();
			var rootSlashCommandGroups = baseSlashCommands.OfType<SlashCommandGroup>().ToList();

			var applicationCommands = new List<DiscordApplicationCommand>();
			applicationCommands.AddRange(GetApplicationCommands(rootSlashCommands));
			applicationCommands.AddRange(GetApplicationCommands(rootSlashCommandGroups));
			return applicationCommands;
		}

		private static List<DiscordApplicationCommand> GetApplicationCommands(List<SlashCommandGroup> slashCommandGroups)
		{
			var applicationCommands = slashCommandGroups
				.Select(slashCommandGroup =>
				{
					var subCommands = GetApplicationSubCommands(slashCommandGroup.Children);
					return new DiscordApplicationCommand(slashCommandGroup.Name, slashCommandGroup.Description, subCommands);
				})
				.ToList();

			return applicationCommands;
		}

		private static List<DiscordApplicationCommandOption> GetApplicationSubCommands(List<SlashCommand> slashCommands)
		{
			var subCommands = slashCommands
				.Select(slashCommand =>
				{
					var commandOptions = GetApplicationCommandOptions(slashCommand);
					return new DiscordApplicationCommandOption(slashCommand.Name, slashCommand.Description, ApplicationCommandOptionType.SubCommand, null, null, commandOptions);
				}).ToList();

			return subCommands;
		}


		private static Dictionary<Type, ApplicationCommandOptionType> ApplicationCommandOptionTypeMap = new Dictionary<Type, ApplicationCommandOptionType>
		{
			[typeof(string)] = ApplicationCommandOptionType.String,
			[typeof(bool)] = ApplicationCommandOptionType.Boolean,
			[typeof(int)] = ApplicationCommandOptionType.Integer
		};

		private static List<DiscordApplicationCommandOption> GetApplicationCommandOptions(SlashCommand slashCommand)
		{
			var parameters = slashCommand.Parameters;

			var commandOptions = parameters.Select(parameter =>
			{
				var nullableType = typeof(int?);
				var parameterType = parameter.ParameterType.Name != nullableType.Name ? parameter.ParameterType : parameter.ParameterType.GenericTypeArguments.Single();

				ApplicationCommandOptionType commandOptionType;
				List<DiscordApplicationCommandOptionChoice> choices = null;
				if (ApplicationCommandOptionTypeMap.ContainsKey(parameterType))
				{
					commandOptionType = ApplicationCommandOptionTypeMap[parameterType];
				}
				else if (parameterType.IsEnum)
				{
					commandOptionType = ApplicationCommandOptionType.String;

					var names = Enum.GetNames(parameterType);
					choices = names.Select(name => new DiscordApplicationCommandOptionChoice(name, name)).ToList();
				}
				else
				{
					throw new NotSupportedException($"Parameter type {parameterType.Name} not yet supported");
				}

				return new DiscordApplicationCommandOption(parameter.Name, parameter.Name, commandOptionType, !parameter.IsOptional, choices);
			}).ToList();

			return commandOptions;
		}

		private static List<DiscordApplicationCommand> GetApplicationCommands(List<SlashCommand> slashCommands)
		{
			var applicationCommands = slashCommands
				.Select(slashCommand =>
				{
					var commandOptions = GetApplicationCommandOptions(slashCommand);
					return new DiscordApplicationCommand(slashCommand.Name, slashCommand.Description, commandOptions);
				})
				.ToList();

			return applicationCommands;
		}
	}
}

using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;
using PizzaBotGG.App.DiscordSlashCommandModule.Attributes;
using PizzaBotGG.App.DiscordSlashCommandModule.Models;
using PizzaBotGG.App.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaBotGG.App.DiscordSlashCommandModule
{
	public class SlashModuleRunner
	{
		public async Task RunModule(SlashContext context)
		{
			var data = context.Interaction.Data;
			var baseSlashCommands = context.BaseSlashCommands;

			var commandName = data.Name;

			//Setup module
			var baseCommand = baseSlashCommands.SingleOrDefault(baseCommand => string.Equals(baseCommand.Name, commandName, StringComparison.OrdinalIgnoreCase));

			if (baseCommand == null) throw new SlashCommandException("Command does not exist");

			var moduleType = baseCommand.ModuleType;
			var module = (SlashModule)ActivatorUtilities.CreateInstance(context.ServiceProvider, moduleType);
			module.SlashContext = context;

			//Run filters
			var slashCommandContext = GetSlashCommandContext(baseCommand, data);
			var slashCommand = slashCommandContext.Command;
			var method = slashCommand.MethodInfo;

			var moduleFilters = moduleType.GetCustomAttributes(false)
				.Select(attribute => attribute as SlashFilterAttribute)
				.Where(filterAttribute => filterAttribute != null).ToList();

			var commandFilters = method.GetCustomAttributes(false)
				.Select(attribute => attribute as SlashFilterAttribute)
				.Where(filterAttribute => filterAttribute != null).ToList();

			var filters = new List<SlashFilterAttribute>();
			filters.AddRange(moduleFilters);
			filters.AddRange(commandFilters);

			foreach (var filter in filters)
			{
				await filter.OnExecuting(context);
			}

			await RunMethod(context, module, slashCommandContext, method);

			filters.Reverse();
			foreach (var filter in filters)
			{
				await filter.OnExecuted(context);
			}
		}

		private async Task RunMethod(SlashContext context, SlashModule module, SlashCommandContext slashCommandContext, System.Reflection.MethodInfo method)
		{
			if (!context.Interaction.Channel.IsNSFW && slashCommandContext.Command.IsNsfw)
			{
				await context.RespondAsync("Wrong channel BAKA!");
				return;
			}

			var invokeTask = (Task)method.Invoke(module, slashCommandContext.Parameters);
			await invokeTask.ConfigureAwait(false);
			var prop = invokeTask.GetType().GetProperty("Result");
			var taskResult = prop.GetValue(invokeTask);

			if (taskResult is string stringResult)
			{
				var actualResult = string.IsNullOrEmpty(stringResult) ? "No result" : stringResult;
				
				var builder = new DiscordEmbedBuilder();
				builder.WithDescription(actualResult);
				var stringEmbed = builder.Build();
				
				await context.RespondAsync(stringEmbed);
				return;
			}

			if (taskResult is DiscordEmbed embed)
			{
				await context.RespondAsync(embed);
				return;
			}

			throw new NotSupportedException($"Result type {taskResult.GetType().Name} of method {method.Name} is not supported");
		}

		private SlashCommandContext GetSlashCommandContext(BaseSlashCommand baseSlashCommand, DiscordInteractionData discordInteractionData)
		{
			switch (baseSlashCommand)
			{
				case SlashCommandGroup slashCommandGroup:
					var dataOption = discordInteractionData.Options.SingleOrDefault();
					if (dataOption == null || dataOption.Type != ApplicationCommandOptionType.SubCommand) throw new SlashCommandException("Command does not exist");
					var command = slashCommandGroup.Children.SingleOrDefault(command => command.Name.Equals(dataOption.Name));
					var commandOptions = dataOption.Options?.ToList() ?? new List<DiscordInteractionDataOption>();
					return GetSlashCommandContext(command, commandOptions);
				case SlashCommand slashCommand:

					var slashCommandOptions = discordInteractionData.Options?.ToList() ?? new List<DiscordInteractionDataOption>();
					return GetSlashCommandContext(slashCommand, slashCommandOptions);
			}

			throw new NotSupportedException($"Command of type {baseSlashCommand.GetType().Name} not supported");
		}

		private SlashCommandContext GetSlashCommandContext(SlashCommand command, List<DiscordInteractionDataOption> options)
		{
			var parameterValues = command.Parameters.Select(parameter =>
			{
				var parameterOption = options.FirstOrDefault(x => x.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase));
				if (parameterOption == null && !parameter.IsOptional) throw new SlashCommandException($"Command requires parameter {parameter.Name}");

				if (parameterOption == null) return null;

				var isNullableEnum = "Nullable`1" == parameter.ParameterType.Name && parameter.ParameterType.GenericTypeArguments.Single().IsEnum;
				if (parameter.ParameterType.IsEnum || isNullableEnum)
				{
					var enumType = isNullableEnum ? parameter.ParameterType.GenericTypeArguments.Single() : parameter.ParameterType;

					var stringValue = parameterOption.Value as string;

					//If parameter option from discord did have a value but the parsed to string variant did not it means it was not a valid type
					if (parameterOption.Value != null && stringValue == null) throw new SlashCommandException($"Expected parameter of type string");

					//If not a valid enum throw exception
					if (!Enum.TryParse(enumType, stringValue, true, out object result)) throw new SlashCommandException($"Given parameter was not one of the given options");

					return result;
				}

				return parameterOption.Value;
			}).ToArray();

			return new SlashCommandContext(command, parameterValues);
		}
	}
}

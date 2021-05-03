using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;
using PizzaBotGG.App.DiscordSlashCommandModule.Interfaces;
using PizzaBotGG.App.DiscordSlashCommandModule.Models;
using PizzaBotGG.App.DiscordSlashCommandModule.Utilities;
using PizzaBotGG.App.ExceptionHandling;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaBotGG.App.DiscordSlashCommandModule
{
	public class SlashCommandService
	{
		private readonly DiscordClient _client;
		private readonly SlashCommandConfiguration _configuration;
		private readonly List<BaseSlashCommand> _baseSlashCommands;
		private readonly ServiceProvider _serviceProvider;
		private readonly SlashCommandExceptionMiddleware _exceptionMiddleware;

		public SlashCommandService(
			DiscordClient client, 
			SlashCommandConfiguration configuration)
		{
			_client = client;
			_configuration = configuration;
			_baseSlashCommands = BuildBaseSlashCommands();
			_serviceProvider = _configuration.Services.BuildServiceProvider();
			_exceptionMiddleware = new SlashCommandExceptionMiddleware(_configuration.OnExceptionHandlers);
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
			_client.InteractionCreated += OnInteractionCreated;
		}

		private async Task OnInteractionCreated(DiscordClient sender, DSharpPlus.EventArgs.InteractionCreateEventArgs e)
		{
			var builder = new DiscordInteractionResponseBuilder();
			builder.WithContent("Processing command");
			await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);

			var task = Task.Run(async () => await RunPipeline(sender, e.Interaction));
		}

		private async Task RunPipeline(DiscordClient client, DiscordInteraction interaction)
		{
			using (var scope = _serviceProvider.CreateScope())
			{
				var scopedProvider = scope.ServiceProvider;
				var slashContext = new SlashContext(
					interaction, 
					_baseSlashCommands,
					scopedProvider,
					client);
				
				var middlewareInstances = new List<ISlashCommandMiddleware>();
				middlewareInstances.Add(_exceptionMiddleware);

				var customMiddleware = scopedProvider.GetServices<ISlashCommandMiddleware>();
				middlewareInstances.AddRange(customMiddleware);

				var moduleRunner = new SlashModuleRunner();
				var middleWareRunner = new SlashCommandMiddlewareRunner(middlewareInstances, moduleRunner.RunModule);
				await middleWareRunner.Run(slashContext);
			}
		}
	}
}

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PizzaBotGG.App.ExceptionHandling;
using PizzaBotGG.App.Modules.Cat.Extensions;
using PizzaBotGG.App.Modules.Music.Extensions;
using PizzaBotGG.App.Modules.Waifu.Extensions;
using PizzaBotGG.App.Services;
using PizzaBotGG.App.Settings;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using PizzaBotGG.App.DiscordSlashCommandModule;

namespace PizzaBotGG.App
{
    public class Program
    {
        static void Main(string[] args)
		{
            MainAsync().Wait();
		}

        static async Task MainAsync()
		{
            var applicationConfiguration = GetConfiguration();
            var discordSettings = applicationConfiguration.GetSection(nameof(DiscordSettings)).Get<DiscordSettings>();
            var lavalinkSettings = applicationConfiguration.GetSection(nameof(LavalinkSettings)).Get<LavalinkSettings>();

            var discordConfiguration = new DiscordConfiguration()
            {
                Token = discordSettings.Token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            };

			var discordClient = new DiscordClient(discordConfiguration);
            var slashCommandService = await discordClient.AddSlashCommands();

            var endpoint = new ConnectionEndpoint
            {
                Hostname = "127.0.0.1",
                Port = 2333
            };

            var lavalinkConfig = new LavalinkConfiguration
            {
                Password = lavalinkSettings.Password,
                RestEndpoint = endpoint,
                SocketEndpoint = endpoint
            };

            var lavalink = discordClient.UseLavalink();
            await discordClient.ConnectAsync();
            await lavalink.ConnectAsync(lavalinkConfig);
            await Task.Delay(-1);
		}

		static ServiceProvider GetServiceProvider()
		{
            var services = new ServiceCollection();
            services.AddWaifuModule();
            services.AddCatModule();
            services.AddMusicModule();
            services.AddSingleton<IRandomService, RandomService>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

		static IConfiguration GetConfiguration()
		{
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Configuration/appsettings.json", optional: false)
                .AddJsonFile($"Configuration/User/appsettings.{Environment.MachineName}.json", optional: true)
                .AddJsonFile($"Configuration/User/appsettings.{Environment.UserName}.json", optional: true)
                .AddJsonFile($"Configuration/Environment/appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddEnvironmentVariables();

            return configurationBuilder.Build();
        }

	}
}

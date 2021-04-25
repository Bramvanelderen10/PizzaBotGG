using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using PizzaBotGG.App.ApiClients.CatApi;
using RestEase;
using System.Reflection;
using System.Threading.Tasks;

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
            var discordConfiguration = new DiscordConfiguration()
            {
                Token = "ODM1ODc0MzcxMDU0NDY5MTQx.YIVyqw.gl-ahRmOuhN3Y2l_nn7-XQtO6wk",
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
            };

            var discordClient = new DiscordClient(discordConfiguration);
            

            var commandConfiguration = new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "/", "/cat" },
                Services = GetServiceProvider()
            };

            var commandsNext = discordClient.UseCommandsNext(commandConfiguration);
            commandsNext.RegisterCommands(Assembly.GetEntryAssembly());
            await discordClient.ConnectAsync();
            await Task.Delay(-1);
		}

        static ServiceProvider GetServiceProvider()
		{
            var services = new ServiceCollection();

            var catApi = RestClient.For<ICatApi>("https://api.thecatapi.com/v1/");
            services.AddSingleton(catApi);
            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
	}
}

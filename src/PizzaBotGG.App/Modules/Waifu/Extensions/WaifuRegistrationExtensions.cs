using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using PizzaBotGG.App.Converters;
using PizzaBotGG.App.Modules.Waifu.Apis;
using PizzaBotGG.App.Modules.Waifu.Enums;
using PizzaBotGG.App.Modules.Waifu.Services;
using RestEase;

namespace PizzaBotGG.App.Modules.Waifu.Extensions
{
	public static class WaifuRegistrationExtensions
	{
		public static IServiceCollection AddWaifuModule(this IServiceCollection services)
		{
			var waifuApi = new RestClient("https://waifu.pics/api/")
			{
				RequestPathParamSerializer = new WaifuApiPathSerializer()
			}.For<IWaifuAPI>();
			services.AddSingleton(waifuApi);
			services.AddTransient<IWaifuService, WaifuService>();

			return services;
		}

		public static CommandsNextExtension RegisterWaifuConverters(this CommandsNextExtension commandsNext)
		{
			commandsNext.RegisterConverter(new NullableEnumConverter<WaifuSFWCategory>());
			commandsNext.RegisterConverter(new NullableEnumConverter<WaifuNSFWCategory>());

			return commandsNext;
		}
	}
}

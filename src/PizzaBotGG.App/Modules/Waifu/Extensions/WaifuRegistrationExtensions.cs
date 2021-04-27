using Microsoft.Extensions.DependencyInjection;
using PizzaBotGG.App.Modules.Waifu.Api;
using PizzaBotGG.App.Modules.Waifu.Service;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}

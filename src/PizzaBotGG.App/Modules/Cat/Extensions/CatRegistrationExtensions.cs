using Microsoft.Extensions.DependencyInjection;
using PizzaBotGG.App.Modules.Cat.Api;
using PizzaBotGG.App.Modules.Cat.Services;
using RestEase;

namespace PizzaBotGG.App.Modules.Cat.Extensions
{
	public static class CatRegistrationExtensions
	{
		public static IServiceCollection AddCatModule(this IServiceCollection services)
		{
			var catApi = RestClient.For<ICatApi>("https://api.thecatapi.com/v1/");
			services.AddSingleton(catApi);
			services.AddTransient<ICatService, CatService>();
			return services;
		}
	}
}

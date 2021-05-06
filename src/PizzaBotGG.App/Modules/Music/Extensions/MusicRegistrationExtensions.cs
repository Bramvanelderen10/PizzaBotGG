using Microsoft.Extensions.DependencyInjection;
using PizzaBotGG.App.Modules.Music.Services;

namespace PizzaBotGG.App.Modules.Music.Extensions
{
	public static class MusicRegistrationExtensions
	{
		public static IServiceCollection AddMusicModule(this IServiceCollection services)
		{
			services.AddSingleton<LavalinkService>();
			services.AddSingleton<IMusicService, MusicService>();
			return services;
		}
	}
}

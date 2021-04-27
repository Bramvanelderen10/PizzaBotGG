using DSharpPlus.Entities;
using PizzaBotGG.App.ExceptionHandling;
using PizzaBotGG.App.Modules.Waifu.Apis;
using PizzaBotGG.App.Modules.Waifu.Enums;
using PizzaBotGG.App.Services;
using System;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Waifu.Services
{
	public class WaifuService : IWaifuService
	{
		private readonly IWaifuAPI _waifuApi;
		private readonly IRandomService _randomService;

		public WaifuService(
			IWaifuAPI waifuApi,
			IRandomService randomService)
		{
			_waifuApi = waifuApi;
			_randomService = randomService;
		}

		public async Task<DiscordEmbed> GetNSFWWaifuEmbed(WaifuNSFWCategory? category = null)
		{
			var waifuCategory = category ?? GetRandomCategory<WaifuNSFWCategory>();
			var waifuResponse = await _waifuApi.GetNSFWWaifu(waifuCategory);
			return GetWaifuEmbed(waifuResponse);
		}

		public async Task<DiscordEmbed> GetSFWWaifuEmbed(WaifuSFWCategory? category = null)
		{
			var waifuCategory = category ?? GetRandomCategory<WaifuSFWCategory>();
			try
			{
				var waifuResponse = await _waifuApi.GetSFWWaifu(waifuCategory);
				return GetWaifuEmbed(waifuResponse);
			}
			catch (RestEase.ApiException apiException)
			{
				if (apiException.StatusCode == System.Net.HttpStatusCode.NotFound)
				{
					throw new CommandException("Waifu category not found");
				}

				throw;
			}
		}

		private static DiscordEmbed GetWaifuEmbed(WaifuResponse waifuResponse)
		{
			var builder = new DiscordEmbedBuilder();
			builder.ImageUrl = waifuResponse.Url;
			var embed = builder.Build();

			return embed;
		}

		private TWaifuCategory GetRandomCategory<TWaifuCategory>()
			where TWaifuCategory : struct, Enum
		{
			var categories = Enum.GetValues<TWaifuCategory>();
			var randomIndex = _randomService.Random(0, categories.Length - 1);
			return categories[randomIndex];
		}
	}
}

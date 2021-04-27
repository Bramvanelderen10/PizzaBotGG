using DSharpPlus.Entities;
using PizzaBotGG.App.Modules.Waifu.Enums;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Waifu.Services
{
	public interface IWaifuService
	{
		Task<DiscordEmbed> GetSFWWaifuEmbed(WaifuSFWCategory? category = null);
		Task<DiscordEmbed> GetNSFWWaifuEmbed(WaifuNSFWCategory? category = null);
	}
}

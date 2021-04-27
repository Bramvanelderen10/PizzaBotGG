using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Cat.Services
{
	public interface ICatService
	{
		Task<string> GetBreedsResponse(string breedName);
		Task<DiscordEmbed> GetCatEmbed(string[] mimeTypes, string breed = null);
	}
}
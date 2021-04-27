using PizzaBotGG.App.Modules.Waifu.Enums;
using RestEase;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Waifu.Apis
{
	public interface IWaifuAPI
	{
		[Get("sfw/{category}")]
		Task<WaifuResponse> GetSFWWaifu([Path("category", PathSerializationMethod.Serialized)] WaifuSFWCategory category);

		[Get("nsfw/{category}")]
		Task<WaifuResponse> GetNSFWWaifu([Path("category", PathSerializationMethod.Serialized)] WaifuNSFWCategory category);
	}
}

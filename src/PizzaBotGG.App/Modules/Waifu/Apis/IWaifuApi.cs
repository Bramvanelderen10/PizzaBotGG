using PizzaBotGG.App.Modules.Waifu.Enums;
using RestEase;
using System;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Waifu.Apis
{
	public interface IWaifuAPI
	{
		[Get("{type}/{category}")]
		Task<WaifuResponse> GetWaifu<TWaifuCategory>(
			[Path("type", PathSerializationMethod.Serialized)] WaifuType type,
			[Path("category", PathSerializationMethod.Serialized)] TWaifuCategory category)
			where TWaifuCategory : struct, Enum;
	}
}

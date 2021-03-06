using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Cat.Api
{
	public interface ICatApi
	{
		[Get("images/search")]
		Task<List<CatResponse>> SearchCats([Query("format")] string format = "json", [Query("breed_id")] string breedId = null, [Query("mime_types")] string[] mimeTypes = null);

		[Get("breeds/search")]
		Task<List<BreedResponse>> SearchBreeds([Query("q")] string name);

		[Get("breeds")]
		Task<List<BreedResponse>> GetBreeds([Query("limit")] int limit);
	}
}

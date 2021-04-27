using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Services
{
	public interface IRandomService
	{
		int Random(int min, int max);
	}
}

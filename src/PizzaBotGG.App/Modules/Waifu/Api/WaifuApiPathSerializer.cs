using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Waifu.Api
{
	public class WaifuApiPathSerializer : RequestPathParamSerializer
	{
		public override string SerializePathParam<T>(T value, RequestPathParamSerializerInfo info)
		{
			if (value is Enum)
			{
				return value.ToString().ToLower();
			}
			else
			{
				return value.ToString();
			}
		}
	}
}

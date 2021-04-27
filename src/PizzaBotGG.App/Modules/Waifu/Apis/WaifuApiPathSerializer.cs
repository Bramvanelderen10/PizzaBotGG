using RestEase;
using System;

namespace PizzaBotGG.App.Modules.Waifu.Apis
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

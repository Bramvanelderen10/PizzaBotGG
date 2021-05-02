using System;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Models
{
	public class SlashCommandParameter
	{
		public SlashCommandParameter(string name, Type parameterType, bool isOptional)
		{
			this.Name = name;
			ParameterType = parameterType;
			IsOptional = isOptional;
		}

		public string Name { get; }
		public Type ParameterType { get; }
		public bool IsOptional { get; }
	}
}

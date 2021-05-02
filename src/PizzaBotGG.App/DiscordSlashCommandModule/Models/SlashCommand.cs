using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Models
{
	public class SlashCommand : BaseSlashCommand
	{
		public SlashCommand(
			string name, 
			string description, 
			Type moduleType,
			MethodInfo methodInfo,
			params SlashCommandParameter[] parameters) : base(name, description, moduleType)
		{
			Parameters = parameters.ToList();
			MethodInfo = methodInfo;
		}

		public List<SlashCommandParameter> Parameters { get; set; }
		public MethodInfo MethodInfo { get; }
	}
}

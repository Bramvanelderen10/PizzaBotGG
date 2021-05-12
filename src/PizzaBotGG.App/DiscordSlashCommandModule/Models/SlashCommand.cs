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
			bool isNsfw,
			bool duplicateCommandToRoot,
			params SlashCommandParameter[] parameters) : base(name, description, moduleType)
		{
			Parameters = parameters.ToList();
			MethodInfo = methodInfo;
			IsNsfw = isNsfw;
			DuplicateCommandToRoot = duplicateCommandToRoot;
		}

		public List<SlashCommandParameter> Parameters { get; }
		public MethodInfo MethodInfo { get; }
		public bool IsNsfw { get; }
		public bool DuplicateCommandToRoot { get; }
	}
}

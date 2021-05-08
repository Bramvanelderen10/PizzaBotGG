using System;
using System.Collections.Generic;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Models
{
	public class SlashCommandGroup : BaseSlashCommand
	{
		public SlashCommandGroup(
			string name,
			string description,
			Type moduleType,
			List<SlashCommand> children) : base(name, description, moduleType)
		{
			Children = children;
		}

		public List<SlashCommand> Children { get; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Attributes
{
	public class SlashCommandAttribute : Attribute
	{
		public SlashCommandAttribute(
			string name, 
			string description,
			bool duplicateCommandToRoot = false)
		{
			Name = name;
			Description = description;
			DuplicateCommandToRoot = duplicateCommandToRoot;
		}

		public string Name { get; }
		public string Description { get; }
		public bool DuplicateCommandToRoot { get; }
	}
}

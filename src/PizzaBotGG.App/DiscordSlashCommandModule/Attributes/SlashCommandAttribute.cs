using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Attributes
{
	public class SlashCommandAttribute : Attribute
	{
		public SlashCommandAttribute(string name, string description)
		{
			Name = name;
			Description = description;
		}

		public string Name { get; }
		public string Description { get; }
	}
}

using System;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Attributes
{
	public class SlashCommandGroupAttribute : Attribute
	{
		public SlashCommandGroupAttribute(
			string name, 
			string description)
		{
			Name = name;
			Description = description;
		}

		public string Name { get; }
		public string Description { get; }
	}
}

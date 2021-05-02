using System;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Models
{
	public abstract class BaseSlashCommand
	{
		public BaseSlashCommand(string name, string description, Type moduleType)
		{
			Name = name;
			Description = description;
			ModuleType = moduleType;
		}
		public string Name { get; }
		public string Description { get; }
		public Type ModuleType { get; }
	}
}

using System.Collections.Generic;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Models
{
	public class SlashCommandGroup : BaseSlashCommand
	{
		public SlashCommandGroup(string name, string description, List<SlashCommand> children) : base(name, description)
		{
			Children = children;
		}

		public List<SlashCommand> Children { get; }
	}
}

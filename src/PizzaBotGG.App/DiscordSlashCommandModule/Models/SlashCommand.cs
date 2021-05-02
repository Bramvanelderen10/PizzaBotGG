using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Models
{
	public class SlashCommand : BaseSlashCommand
	{
		public SlashCommand(string name, string description, params SlashCommandParameter[] parameters) : base(name, description)
		{
			Parameters = parameters.ToList();
		}

		public List<SlashCommandParameter> Parameters { get; set; }
	}
}

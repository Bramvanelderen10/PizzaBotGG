using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBotGG.App.DiscordSlashCommandModule
{
	public abstract class SlashModule
	{
		public CommandContext CommandContext { get; set; }
	}
}

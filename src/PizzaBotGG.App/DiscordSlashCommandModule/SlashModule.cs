using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PizzaBotGG.App.DiscordSlashCommandModule
{
	public abstract class SlashModule
	{
		public SlashContext SlashContext { get; set; }
	}
}

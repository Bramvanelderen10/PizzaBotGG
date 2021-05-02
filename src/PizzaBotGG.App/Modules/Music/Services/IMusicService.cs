using DSharpPlus.CommandsNext;
using DSharpPlus.Lavalink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Music.Services
{
	public interface IMusicService
	{
		Task Connect(CommandContext context);
		Task Play(CommandContext context, string search);
		Task Skip(CommandContext context);
		Task Pause(CommandContext context);
		Task Unpause(CommandContext context);
		Task Queue(CommandContext context);
		Task Clear(CommandContext context);
		Task Stats(CommandContext context);
	}
}

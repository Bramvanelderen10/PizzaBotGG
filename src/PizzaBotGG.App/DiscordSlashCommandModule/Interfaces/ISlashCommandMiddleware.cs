using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Interfaces
{
	public interface ISlashCommandMiddleware
	{
		Task InvokeAsync(SlashContext context, Func<SlashContext, Task> next);
	}
}

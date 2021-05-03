using System;
using System.Threading.Tasks;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Attributes
{
	public abstract class SlashFilterAttribute : Attribute
	{
		public virtual async Task OnExecuting(SlashContext slashContext) { }
		public virtual async Task OnExecuted(SlashContext slashContext) { }
	}
}

using System.Threading.Tasks;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Attributes
{
	public abstract class SlashCommandFilterAttribute
	{
		public abstract Task OnExecuting(SlashContext slashContext);
		public abstract Task OnExecuted(SlashContext slashContext);
	}
}

using DSharpPlus.CommandsNext;
using System.Threading.Tasks;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Attributes
{
	public abstract class SlashCommandFilterAttribute
	{
		public abstract Task OnExecuting(CommandContext commandContext);
		public abstract Task OnExecuted(CommandContext commandContext);
	}
}

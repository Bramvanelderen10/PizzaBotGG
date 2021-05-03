using PizzaBotGG.App.DiscordSlashCommandModule;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Music.Services
{
	public interface IMusicService
	{
		Task Connect(SlashContext context);
		Task Play(SlashContext context, string search);
		Task Skip(SlashContext context);
		Task Pause(SlashContext context);
		Task Unpause(SlashContext context);
		Task Queue(SlashContext context);
		Task Clear(SlashContext context);
		Task Stats(SlashContext context);
	}
}

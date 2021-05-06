using PizzaBotGG.App.DiscordSlashCommandModule;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Music.Services
{
	public interface IMusicService
	{
		Task<string> Clear(SlashContext context);
        Task<string> Pause(SlashContext context);
        Task<string> Play(SlashContext context, string search);
        Task<string> Queue(SlashContext context);
        Task<string> Skip(SlashContext context);
        string Stats(SlashContext context);
        Task<string> Unpause(SlashContext context);
	}
}

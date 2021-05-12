using PizzaBotGG.App.DiscordSlashCommandModule;
using PizzaBotGG.App.DiscordSlashCommandModule.Attributes;
using PizzaBotGG.App.Modules.Music.Attributes;
using PizzaBotGG.App.Modules.Music.Services;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Music
{
	[SlashCommandGroup("music", "The music module")]
	[MusicFilter]
	public class MusicModule : SlashModule
	{
		private readonly IMusicService _musicService;

		public MusicModule(IMusicService musicService)
		{
			_musicService = musicService;
		}

		[SlashCommand("play", "Plays music using either a song name or link", true)]
		public async Task<string> Play(string search) => await _musicService.Play(SlashContext, search);

		[SlashCommand("skip", "Skips a song")]
		public async Task<string> Skip() => await _musicService.Skip(SlashContext);

		[SlashCommand("pause", "Pause a song")]
		public async Task<string> Pause() => await _musicService.Pause(SlashContext);

		[SlashCommand("unpause", "Unpause a song")]
		public async Task<string> Unpause() => await _musicService.Unpause(SlashContext);

		[SlashCommand("queue", "Queue a song")]
		public async Task<string> Queue() => await _musicService.Queue(SlashContext);

		[SlashCommand("clear", "Clear the queue")]
		public async Task<string> Clear() => await _musicService.Clear(SlashContext);

		[SlashCommand("stats", "Displays Lavalink statistics.")]
		public async Task<string> Stats() => _musicService.Stats(SlashContext);
	}
}

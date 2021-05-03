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

		[SlashCommand("play", "Plays music using either a song name or link")]
		public async Task<string> Play(string search)
		{
			await _musicService.Play(SlashContext, search);
			return "Success";
		}

		[SlashCommand("skip", "Skips a song")]
		public async Task<string> Skip()
		{
			await _musicService.Skip(SlashContext);
			return "Success";
		}

		[SlashCommand("pause", "Pause a song")]
		public async Task<string> Pause()
		{
			await _musicService.Pause(SlashContext);
			return "Success";
		}

		[SlashCommand("unpause", "Unpause a song")]
		public async Task<string> Unpause()
		{
			await _musicService.Unpause(SlashContext);
			return "Success";
		}

		[SlashCommand("queue", "Queue a song")]
		public async Task<string> Queue()
		{
			await _musicService.Queue(SlashContext);
			return "Success";
		}

		[SlashCommand("clear", "Clear the queue")]
		public async Task<string> Clear()
		{
			await _musicService.Clear(SlashContext);
			return "Success";
		}

		[SlashCommand("stats", "Displays Lavalink statistics.")]
		public async Task<string> Stats()
		{
			await _musicService.Stats(SlashContext);
			return "Success";
		}

	}
}

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using PizzaBotGG.App.Modules.Music.Services;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Music
{
	public class MusicModule : BaseCommandModule
	{
		private readonly IMusicService _musicService;

		public MusicModule(IMusicService musicService)
		{
			_musicService = musicService;
		}

		public override async Task BeforeExecutionAsync(CommandContext context)
		{
			await _musicService.Connect(context);
			await base.BeforeExecutionAsync(context);
		}

		[Command]
		public async Task Play(CommandContext context, [RemainingText] string search)
		{
			await _musicService.Play(context, search);
		}

		[Command]
		public async Task Skip(CommandContext context)
		{
			await _musicService.Skip(context);
		}

		[Command]
		public async Task Pause(CommandContext context)
		{
			await _musicService.Pause(context);
		}

		[Command]
		public async Task Unpause(CommandContext context)
		{
			await _musicService.Unpause(context);
		}

		[Command]
		public async Task Queue(CommandContext context)
		{
			await _musicService.Queue(context);
		}

		[Command]
		public async Task Clear(CommandContext context)
		{
			await _musicService.Clear(context);
		}

		[Command, Description("Displays Lavalink statistics.")]
		public async Task Stats(CommandContext context)
		{
			await _musicService.Stats(context);
		}

	}
}

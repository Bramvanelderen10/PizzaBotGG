using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using System.Collections.Generic;

namespace PizzaBotGG.App.Modules.Music.Services
{
	public class GuildContext
	{
		public ulong GuildId { get; }
		public List<LavalinkTrack> Queue { get; }
		public DiscordChannel OriginalChannel { get; }
		public int QueueIndex { get; set; } = 0;
		public bool IsLooping { get; set; } = false;

		public GuildContext(ulong guildId, DiscordChannel originalChannel)
		{
			GuildId = guildId;
			Queue = new List<LavalinkTrack>();
			OriginalChannel = originalChannel;
		}

		public void Reset()
		{
			QueueIndex = 0;
			IsLooping = false;
		}
	}
}

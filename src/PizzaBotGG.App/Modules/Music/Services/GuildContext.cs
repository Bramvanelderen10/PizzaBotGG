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

		public GuildContext(ulong guildId, DiscordChannel originalChannel)
		{
			GuildId = guildId;
			Queue = new List<LavalinkTrack>();
			OriginalChannel = originalChannel;
		}
	}
}

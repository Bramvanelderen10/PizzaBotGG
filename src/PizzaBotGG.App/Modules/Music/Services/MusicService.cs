using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;
using PizzaBotGG.App.DiscordSlashCommandModule;
using PizzaBotGG.App.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Music.Services
{

	public class LavalinkService
	{

		public LavalinkNodeConnection LavalinkNodeConnection { get; private set; }
		public Dictionary<ulong, LavalinkGuildConnection> LavalinkGuildConnections { get; private set; }
		private readonly List<Func<LavalinkGuildConnection, TrackFinishEventArgs, Task>> _onPlaybackFinishedListeners;

		public LavalinkService()
		{
			LavalinkGuildConnections = new Dictionary<ulong, LavalinkGuildConnection>();
			_onPlaybackFinishedListeners = new List<Func<LavalinkGuildConnection, TrackFinishEventArgs, Task>>();
		}

		public async Task Connect(SlashContext context)
		{
			var userId = context.Interaction.User.Id;
			var guild = context.Interaction.Guild;

			if (!guild.VoiceStates.ContainsKey(userId)) throw new SlashCommandException("You are not in a voice channel");

			var voiceState = guild.VoiceStates[userId];

			if (voiceState.Channel == null) throw new SlashCommandException("You are not in a voice channel");

			var client = context.Client;

			LavalinkNodeConnection = await GetLavalinkNodeConnection(context, voiceState);

			//If there is already a guild connection then we are done
			if (LavalinkGuildConnections.ContainsKey(guild.Id)) return;


			var guildConnection = LavalinkNodeConnection.GetGuildConnection(guild);
			if (guildConnection == null) throw new SlashCommandException("Lavalink is not connected.");
			guildConnection.PlaybackFinished += PlaybackFinishedHandler;
			LavalinkGuildConnections[guild.Id] = LavalinkNodeConnection.GetGuildConnection(guild);
		}

		public void AddOnPlaybackFinishedListener(Func<LavalinkGuildConnection, TrackFinishEventArgs, Task> onPlaybackFinishedListener)
		{
			_onPlaybackFinishedListeners.Add(onPlaybackFinishedListener);
		}

		private async Task PlaybackFinishedHandler(LavalinkGuildConnection sender, TrackFinishEventArgs e)
		{
			foreach (var listener in _onPlaybackFinishedListeners)
			{
				await listener.Invoke(sender, e);
			}
		}

		private async Task<LavalinkNodeConnection> GetLavalinkNodeConnection(SlashContext context, DiscordVoiceState voiceState)
		{
			//Connection already establish just return it
			if (LavalinkNodeConnection != null) return LavalinkNodeConnection;

			var lava = context.Client.GetLavalink();
			var channel = voiceState.Channel;

			if (channel.Type != ChannelType.Voice) throw new SlashCommandException("Not a valid voice channel.");

			if (!lava.ConnectedNodes.Any()) throw new SlashCommandException("The Lavalink connection is not established");

			var lavalinkNodeConnection = lava.ConnectedNodes.Values.First();

			//If already connected to a guild just return the connection
			if (lavalinkNodeConnection.ConnectedGuilds.Any()) return lavalinkNodeConnection;

			await lavalinkNodeConnection.ConnectAsync(channel);

			return lavalinkNodeConnection;
		}
	}


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

	public class MusicService : IMusicService
	{
		private readonly List<GuildContext> _guildContexts;
		private readonly LavalinkService _lavalinkService;
		public MusicService(LavalinkService lavalinkService)
		{
			_lavalinkService = lavalinkService;
			_lavalinkService.AddOnPlaybackFinishedListener(PlaybackFinishedHandler);
			_guildContexts = new List<GuildContext>();
		}


		public async Task<string> Play(SlashContext context, string search)
		{
			if (string.IsNullOrWhiteSpace(search)) throw new SlashCommandException("search something noob");

			var connection = _lavalinkService.LavalinkNodeConnection;
			var loadResult = await connection.Rest.GetTracksAsync(search);

			//Check if load failed or no load matches. if so throw
			var hasLoadFailed = loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed;
			var hasNoLoadMatches = loadResult.LoadResultType == LavalinkLoadResultType.NoMatches;
			if (hasLoadFailed || hasNoLoadMatches) throw new SlashCommandException($"Track search failed for {search}.");

			var guild = context.Interaction.Guild;
			var guildConnection = _lavalinkService.LavalinkGuildConnections[guild.Id];

			var guildContext = CreateGuildContext(guildConnection, context);
			var track = loadResult.Tracks.First();

			if (guildConnection.CurrentState.CurrentTrack == null)
			{
				await guildConnection.PlayAsync(track);


				return $"Now playing {GetTrackDescription(track)}";
			}
			else
			{
				guildContext.Queue.Add(track);
				return $"Queued {GetTrackDescription(track)}!";
			}
		}

		private static string GetTrackDescription(LavalinkTrack track) => $"Now playing {Formatter.MaskedUrl(track.Title, track.Uri)}!";

		private GuildContext CreateGuildContext(LavalinkGuildConnection guildConnection, SlashContext context)
		{
			var guild = guildConnection.Guild;
			var guildContext = _guildContexts.SingleOrDefault(guildContext => guildContext.GuildId == guild.Id);

			if (guildContext != null) return guildContext;

			var channel = context.Interaction.Channel;
			var newGuildContext = new GuildContext(guild.Id, channel);

			_guildContexts.Add(newGuildContext);

			return newGuildContext;
		}

		private GuildContext GetGuildContext(LavalinkGuildConnection guildConnection)
		{
			var guild = guildConnection.Guild;
			var guildContext = _guildContexts.SingleOrDefault(guildContext => guildContext.GuildId == guild.Id);

			if (guildContext == null) throw new SlashCommandException("No guild connection");

			return guildContext;
		}

		private async Task PlaybackFinishedHandler(LavalinkGuildConnection sender, TrackFinishEventArgs e)
		{
			if (e.Reason != TrackEndReason.Finished) return;

			await NextTrack(sender);
		}

		private async Task NextTrack(LavalinkGuildConnection guildConnection)
		{
			var guild = guildConnection.Guild;
			var guildContext = GetGuildContext(guildConnection);

			//If guild queue any then dont queue anything
			if (!guildContext.Queue.Any())
			{
				await guildContext.OriginalChannel.SendMessageAsync("Queue finished");
				return;
			}

			var nextTrack = guildContext.Queue.FirstOrDefault();
			guildContext.Queue.Remove(nextTrack);
			await guildConnection.PlayAsync(nextTrack);
			var nowPlaying = GetTrackDescription(nextTrack);
			await guildContext.OriginalChannel.SendMessageAsync($"Playing: {GetTrackDescription(nextTrack)}");
		}

		public async Task<string> Pause(SlashContext context)
		{
			var guildConnection = GetGuildConnection(context);
			await guildConnection.PauseAsync();

			return "Paused";
		}

		public async Task<string> Unpause(SlashContext context)
		{
			var guildConnection = GetGuildConnection(context);
			await guildConnection.ResumeAsync();

			return "Unpaused";
		}

		private LavalinkGuildConnection GetGuildConnection(SlashContext context)
		{
			var guildId = context.Interaction.GuildId.Value;
			var guildConnection = _lavalinkService.LavalinkGuildConnections[guildId];
			if (guildConnection.CurrentState.CurrentTrack == null) throw new SlashCommandException("There are no tracks loaded.");

			return guildConnection;
		}

		public async Task<string> Skip(SlashContext context)
		{
			var guildConnection = GetGuildConnection(context);

			await NextTrack(guildConnection);
			return "Skipping";
		}

		public async Task<string> Queue(SlashContext context)
		{
			var guildConnection = GetGuildConnection(context);
			var guildContext = GetGuildContext(guildConnection);

			var queue = guildContext.Queue;

			if (!queue.Any()) throw new SlashCommandException("There are no tracks queued");

			var stringBuilder = new StringBuilder();
			stringBuilder.Append("So you wanna know what's playing yes? Check out the list: ");
			foreach (var track in queue)
			{
				stringBuilder.Append($"{track.Title}\t\t{track.Length}\n").AppendLine();
			}
			stringBuilder.Append("");

			return stringBuilder.ToString();
		}

		public async Task<string> Clear(SlashContext context)
		{
			var guildConnection = GetGuildConnection(context);
			var guildContext = GetGuildContext(guildConnection);
			var queue = guildContext.Queue;

			queue.Clear();
			await guildConnection.StopAsync();
			return "Queue cleared";
		}

		public string Stats(SlashContext context)
		{
			var stats = _lavalinkService.LavalinkNodeConnection.Statistics;
			var sb = new StringBuilder();
			sb.Append("Lavalink resources usage statistics: ")
				.Append("Uptime:                    ").Append(stats.Uptime).AppendLine()
				.Append("Players:                   ").AppendFormat("{0} active / {1} total", stats.ActivePlayers, stats.TotalPlayers).AppendLine()
				.Append("CPU Cores:                 ").Append(stats.CpuCoreCount).AppendLine()
				.Append("CPU Usage:                 ").AppendFormat("{0:#,##0.0%} lavalink / {1:#,##0.0%} system", stats.CpuLavalinkLoad, stats.CpuSystemLoad).AppendLine()
				.Append("RAM Usage:                 ").AppendFormat("{0} allocated / {1} used / {2} free / {3} reservable", SizeToString(stats.RamAllocated), SizeToString(stats.RamUsed), SizeToString(stats.RamFree), SizeToString(stats.RamReservable)).AppendLine()
				.Append("Audio frames (per minute): ").AppendFormat("{0:#,##0} sent / {1:#,##0} nulled / {2:#,##0} deficit", stats.AverageSentFramesPerMinute, stats.AverageNulledFramesPerMinute, stats.AverageDeficitFramesPerMinute).AppendLine()
				.Append("");

			return sb.ToString();
		}

		private static string[] Units = new[] { "", "ki", "Mi", "Gi" };

		private static string SizeToString(long l)
		{
			double d = l;
			int u = 0;
			while (d >= 900 && u < Units.Length - 2)
			{
				u++;
				d /= 1024;
			}

			return $"{d:#,##0.00} {Units[u]}B";
		}
	}
}

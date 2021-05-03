using DSharpPlus;
using DSharpPlus.Lavalink;
using PizzaBotGG.App.DiscordSlashCommandModule;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Music.Services
{
	public class MusicService : IMusicService
	{
		private List<LavalinkTrack> CurrentQueue { get; set; } = new List<LavalinkTrack>();
		private LavalinkNodeConnection LavalinkNodeConnection { get; set; }
		private LavalinkGuildConnection LavalinkGuildConnection { get; set; }

		public async Task Connect(SlashContext context)
		{
			var userId = context.Interaction.User.Id;
			var guild = context.Interaction.Guild;


			if (!guild.VoiceStates.ContainsKey(userId))
			{
				await context.RespondAsync("You are not in a voice channel");
				return;
			}

			var voiceState = guild.VoiceStates[userId];
			if (voiceState.Channel == null)
			{
				await context.RespondAsync("You are not in a voice channel");
				return;
			}

			var client = context.Client;

			if (LavalinkNodeConnection == null)
			{
				var lava = context.Client.GetLavalink();
				if (!lava.ConnectedNodes.Any())
				{
					await context.RespondAsync("The Lavalink connection is not established");
					return;
				}
				LavalinkNodeConnection = lava.ConnectedNodes.Values.First();

				if (!LavalinkNodeConnection.ConnectedGuilds.Any())
				{
					var channel = voiceState.Channel;

					if (!lava.ConnectedNodes.Any())
					{
						await context.RespondAsync("The Lavalink connection is not established");
						return;
					}

					if (channel.Type != ChannelType.Voice)
					{
						await context.RespondAsync("Not a valid voice channel.");
						return;
					}

					await LavalinkNodeConnection.ConnectAsync(channel);
					await context.RespondAsync($"Joined {channel.Name}!");
				}
			}

			if (LavalinkGuildConnection == null)
			{
				LavalinkGuildConnection = LavalinkNodeConnection.GetGuildConnection(voiceState.Guild);
				if (LavalinkGuildConnection == null)
				{
					await context.RespondAsync("Lavalink is not connected.");
					return;
				}
			}
		}

		public async Task Play(SlashContext context, string search)
		{
			if (string.IsNullOrWhiteSpace(search))
			{
				await context.RespondAsync("```search something noob```");
				return;
			}

			var loadResult = await LavalinkNodeConnection.Rest.GetTracksAsync(search);

			if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed
				|| loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
			{
				await context.RespondAsync($"Track search failed for {search}.");
				return;
			}
			var track = loadResult.Tracks.First();

			if (LavalinkGuildConnection.CurrentState.CurrentTrack == null)
			{
				await LavalinkGuildConnection.PlayAsync(track);

				await context.RespondAsync($"```Now playing {track.Uri}!```");
			}
			else
			{
				CurrentQueue.Add(track);
				LavalinkGuildConnection.PlaybackFinished += LavalinkGuildConnection_PlaybackFinished;
				await context.RespondAsync($"```Queued {track.Title}!```");
			}
		}

		private async Task LavalinkGuildConnection_PlaybackFinished(LavalinkGuildConnection sender, DSharpPlus.Lavalink.EventArgs.TrackFinishEventArgs e)
		{
			if (CurrentQueue.Count > 0)
			{
				var nextTrack = CurrentQueue.FirstOrDefault();
				CurrentQueue.Remove(nextTrack);
				await sender.PlayAsync(nextTrack);
			}
		}

		public async Task Pause(SlashContext context)
		{
			if (LavalinkGuildConnection.CurrentState.CurrentTrack == null)
			{
				await context.RespondAsync("There are no tracks loaded.");
				return;
			}

			await LavalinkGuildConnection.PauseAsync();
		}

		public async Task Unpause(SlashContext context)
		{
			if (LavalinkGuildConnection.CurrentState.CurrentTrack == null)
			{
				await context.RespondAsync("There are no tracks loaded.");
				return;
			}

			await LavalinkGuildConnection.ResumeAsync();
		}

		public async Task Skip(SlashContext context)
		{
			if (LavalinkGuildConnection.CurrentState.CurrentTrack == null)
			{
				await context.RespondAsync("There are no tracks loaded.");
				return;
			}

			if (CurrentQueue.Count > 0)
			{
				var nextTrack = CurrentQueue.FirstOrDefault();
				await LavalinkGuildConnection.PlayAsync(nextTrack);
				CurrentQueue.Remove(nextTrack);

				await context.RespondAsync($"```Now playing {nextTrack.Title}!```");
			}
			else
			{
				await LavalinkGuildConnection.StopAsync();
			}
		}

		public async Task Queue(SlashContext context)
		{
			if (LavalinkGuildConnection.CurrentState.CurrentTrack == null)
			{
				await context.RespondAsync("There are no tracks loaded.");
				return;
			}

			if (CurrentQueue.Count == 0)
			{
				await context.RespondAsync("There are no tracks queued");
			}
			else
			{
				var sb = new StringBuilder();
				sb.Append("So you wanna know what's playing yes? Check out the list: ```");
				foreach (var track in CurrentQueue)
				{
					sb.Append($"{track.Title}\t\t{track.Length}\n").AppendLine();
				}
				sb.Append("```");
				await context.RespondAsync(sb.ToString());
			}
		}

		public async Task Clear(SlashContext context)
		{
			CurrentQueue = new List<LavalinkTrack>();
			await LavalinkGuildConnection.StopAsync();
			await context.RespondAsync("```Queue cleared```");
		}

		public async Task Stats(SlashContext context)
		{
			var stats = LavalinkNodeConnection.Statistics;
			var sb = new StringBuilder();
			sb.Append("Lavalink resources usage statistics: ```")
				.Append("Uptime:                    ").Append(stats.Uptime).AppendLine()
				.Append("Players:                   ").AppendFormat("{0} active / {1} total", stats.ActivePlayers, stats.TotalPlayers).AppendLine()
				.Append("CPU Cores:                 ").Append(stats.CpuCoreCount).AppendLine()
				.Append("CPU Usage:                 ").AppendFormat("{0:#,##0.0%} lavalink / {1:#,##0.0%} system", stats.CpuLavalinkLoad, stats.CpuSystemLoad).AppendLine()
				.Append("RAM Usage:                 ").AppendFormat("{0} allocated / {1} used / {2} free / {3} reservable", SizeToString(stats.RamAllocated), SizeToString(stats.RamUsed), SizeToString(stats.RamFree), SizeToString(stats.RamReservable)).AppendLine()
				.Append("Audio frames (per minute): ").AppendFormat("{0:#,##0} sent / {1:#,##0} nulled / {2:#,##0} deficit", stats.AverageSentFramesPerMinute, stats.AverageNulledFramesPerMinute, stats.AverageDeficitFramesPerMinute).AppendLine()
				.Append("```");
			await context.RespondAsync(sb.ToString()).ConfigureAwait(false);
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

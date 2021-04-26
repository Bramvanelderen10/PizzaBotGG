using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Music
{
	public class MusicModule : BaseCommandModule
	{
		[Command]
		public async Task Play(CommandContext context, [RemainingText] string search)
		{
			if (string.IsNullOrWhiteSpace(search))
			{
				await context.RespondAsync("search something noob");
				return;
			}

			var lava = context.Client.GetLavalink();
			var node = lava.ConnectedNodes.Values.First();


			if(!node.ConnectedGuilds.Any())
			{
				var channel = context.Member.VoiceState.Channel;

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

				await node.ConnectAsync(channel);
				await context.RespondAsync($"Joined {channel.Name}!");
			}

			if (context.Member.VoiceState == null || context.Member.VoiceState.Channel == null)
			{
				await context.RespondAsync("You are not in a voice channel");
				return;
			}

			var conn = node.GetGuildConnection(context.Member.VoiceState.Guild);

			if (conn == null)
			{
				await context.RespondAsync("Lavalink is not connected.");
				return;
			}

			var loadResult = await node.Rest.GetTracksAsync(search);

			if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed
				|| loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
			{
				await context.RespondAsync($"Track search failed for {search}.");
				return;
			}

			var track = loadResult.Tracks.First();

			await conn.PlayAsync(track);

			await context.RespondAsync($"Now playing {track.Title}!");
		}

		[Command]
		public async Task Pause(CommandContext ctx)
		{
			if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
			{
				await ctx.RespondAsync("You are not in a voice channel.");
				return;
			}

			var lava = ctx.Client.GetLavalink();
			var node = lava.ConnectedNodes.Values.First();
			var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

			if (conn == null)
			{
				await ctx.RespondAsync("Lavalink is not connected.");
				return;
			}

			if (conn.CurrentState.CurrentTrack == null)
			{
				await ctx.RespondAsync("There are no tracks loaded.");
				return;
			}

			await conn.PauseAsync();
		}

		[Command]
		public async Task Unpause(CommandContext ctx)
		{
			if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
			{
				await ctx.RespondAsync("You are not in a voice channel.");
				return;
			}

			// TODO generic validation / checks?
			var lava = ctx.Client.GetLavalink();
			var node = lava.ConnectedNodes.Values.First();
			var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

			if (conn == null)
			{
				await ctx.RespondAsync("Lavalink is not connected.");
				return;
			}

			if (conn.CurrentState.CurrentTrack == null)
			{
				await ctx.RespondAsync("There are no tracks loaded.");
				return;
			}

			await conn.ResumeAsync();
		}

	}
}

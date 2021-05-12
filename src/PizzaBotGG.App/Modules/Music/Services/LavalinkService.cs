using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;
using PizzaBotGG.App.DiscordSlashCommandModule;
using PizzaBotGG.App.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Music.Services
{
	public class LavalinkService
	{

		public LavalinkNodeConnection LavalinkNodeConnection { get; private set; }
		private readonly List<Func<LavalinkGuildConnection, TrackFinishEventArgs, Task>> _onPlaybackFinishedListeners;

		public LavalinkService()
		{
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
			if (LavalinkNodeConnection.ConnectedGuilds.ContainsKey(guild.Id)) return;

			var guildConnection = LavalinkNodeConnection.ConnectedGuilds[guild.Id];
			guildConnection.PlaybackFinished += PlaybackFinishedHandler;
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
}

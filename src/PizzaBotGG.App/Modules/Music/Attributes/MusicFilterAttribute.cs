using PizzaBotGG.App.DiscordSlashCommandModule;
using PizzaBotGG.App.DiscordSlashCommandModule.Attributes;
using PizzaBotGG.App.Modules.Music.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Modules.Music.Attributes
{
	public class MusicFilterAttribute : SlashFilterAttribute
	{
		public override async Task OnExecuting(SlashContext slashContext)
		{
			var lavalinkService = slashContext.ServiceProvider.GetService<LavalinkService>();

			await lavalinkService.Connect(slashContext);

			var lavalinkConnection = lavalinkService.LavalinkNodeConnection;

			var musicService = slashContext.ServiceProvider.GetService<IMusicService>();
			var guildConnection = lavalinkConnection.ConnectedGuilds[slashContext.Interaction.Guild.Id];
			musicService.UpdateGuildConnection(guildConnection, slashContext); 
		}
	}
}

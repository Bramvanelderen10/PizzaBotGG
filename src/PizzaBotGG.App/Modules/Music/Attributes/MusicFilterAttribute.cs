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
			var musicService = slashContext.ServiceProvider.GetService<IMusicService>();
			await musicService.Connect(slashContext);
		}    
	}
}

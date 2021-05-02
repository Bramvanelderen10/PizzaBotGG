using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using PizzaBotGG.App.DiscordSlashCommandModule.Attributes;
using PizzaBotGG.App.DiscordSlashCommandModule.Interfaces;
using PizzaBotGG.App.ExceptionHandling;
using System;
using System.Collections.Generic;

namespace PizzaBotGG.App.DiscordSlashCommandModule
{
	public class SlashCommandConfiguration
	{
		public SlashCommandConfiguration()
		{
			GlobalCommandFilters = new List<SlashCommandFilterAttribute>();
			Services = new ServiceCollection();
			OnExceptionHandlers = new List<Action<SlashCommandErrorContext>>();
		}

		public List<SlashCommandFilterAttribute> GlobalCommandFilters { get; }
		public List<Action<SlashCommandErrorContext>> OnExceptionHandlers { get; }
		public IServiceCollection Services { get; }
	}
}

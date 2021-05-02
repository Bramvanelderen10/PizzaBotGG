using PizzaBotGG.App.DiscordSlashCommandModule;
using System;

namespace PizzaBotGG.App.ExceptionHandling
{
	public class SlashCommandErrorContext
	{
		public SlashCommandErrorContext(
			SlashContext context,
			Exception exception)
		{
			Context = context;
			Exception = exception;
			ExceptionHandled = false;
		}

		public SlashContext Context { get; }
		public Exception Exception { get; }
		public bool ExceptionHandled { get; set; }
	}
}

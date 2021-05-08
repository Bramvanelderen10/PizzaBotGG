using PizzaBotGG.App.DiscordSlashCommandModule;
using PizzaBotGG.App.DiscordSlashCommandModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaBotGG.App.ExceptionHandling
{
	public class SlashCommandExceptionMiddleware : ISlashCommandMiddleware
	{
		private readonly List<Action<SlashCommandErrorContext>> _onExceptionHandlers;

		public SlashCommandExceptionMiddleware(List<Action<SlashCommandErrorContext>> onExceptionHandlers = null)
		{
			_onExceptionHandlers = onExceptionHandlers ?? new List<Action<SlashCommandErrorContext>>();
		}

		public async Task InvokeAsync(SlashContext context, System.Func<SlashContext, Task> next)
		{
			try
			{
				await next(context);
			}
			catch (Exception exception)
			{
				var errorContext = new SlashCommandErrorContext(context, exception);
				foreach (var customExceptionHandler in _onExceptionHandlers)
				{
					customExceptionHandler(errorContext);
				}

				if (errorContext.ExceptionHandled)
				{
					return;
				}

				if (exception is SlashCommandException slashCommandException)
				{
					await context.RespondAsync(slashCommandException.Message);
					return;
				}

				//TODO add logging here. All exceptions need to be caught here OR ELSE THE PROGRAM CRASHES
				await context.RespondAsync("Exception occurred");
			}
		}
	}
}

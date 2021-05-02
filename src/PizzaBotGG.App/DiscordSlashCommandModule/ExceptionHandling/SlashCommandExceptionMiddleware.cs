using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using PizzaBotGG.App.DiscordSlashCommandModule;
using PizzaBotGG.App.DiscordSlashCommandModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public async Task HandleCommandException(CommandsNextExtension sender, CommandErrorEventArgs eventArguments)
		{
			var exception = eventArguments.Exception;
			if (!(exception is SlashCommandException commandException)) return;

			await eventArguments.Context.RespondAsync(commandException.Message);
		}

		public async Task InvokeAsync(SlashContext context, System.Func<SlashContext, Task> next)
		{

			try
			{
				await next(context);
			}
			catch(Exception exception)
			{
				var errorContext = new SlashCommandErrorContext(context, exception);
				foreach(var customExceptionHandler in _onExceptionHandlers)
				{
					customExceptionHandler(errorContext);
				}

				if (errorContext.ExceptionHandled)
				{
					return;
				}

				if (exception is SlashCommandException slashCommandException)
				{
					await context.Respond(slashCommandException.Message);
					return;
				}

				throw;
			}
		}
	}
}

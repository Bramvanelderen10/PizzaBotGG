using PizzaBotGG.App.DiscordSlashCommandModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaBotGG.App.DiscordSlashCommandModule
{
	public class SlashCommandMiddlewareRunner
	{
		private readonly Func<SlashContext, Task> _finalFunction;
		private int _currentMiddlewareIndex = 0;
		private readonly List<ISlashCommandMiddleware> _middlewareInstances;
		public SlashCommandMiddlewareRunner(
			List<ISlashCommandMiddleware> middlewareInstances,
			Func<SlashContext, Task> finalFunction)
		{
			_finalFunction = finalFunction;
			_middlewareInstances = middlewareInstances;
		}

		public async Task Run(SlashContext context)
		{
			if (_middlewareInstances.Count <= _currentMiddlewareIndex)
			{
				await _finalFunction(context);
				return;
			}

			var currentMiddleware = _middlewareInstances[_currentMiddlewareIndex];
			_currentMiddlewareIndex++;
			await currentMiddleware.InvokeAsync(context, Run);
		}
	}
}

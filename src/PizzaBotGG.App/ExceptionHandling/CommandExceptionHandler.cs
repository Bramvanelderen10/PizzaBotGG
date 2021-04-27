using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaBotGG.App.ExceptionHandling
{
	public class CommandExceptionHandler
	{
		public async Task HandleCommandException(CommandsNextExtension sender, CommandErrorEventArgs eventArguments)
		{
			var exception = eventArguments.Exception;
			if (!(exception is CommandException commandException)) return;

			await eventArguments.Context.RespondAsync(commandException.Message);
		}
	}
}

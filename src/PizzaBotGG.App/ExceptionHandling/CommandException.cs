using System;

namespace PizzaBotGG.App.ExceptionHandling
{
	public class CommandException : Exception
	{
		public CommandException(string message) : base(message)
		{

		}
	}
}

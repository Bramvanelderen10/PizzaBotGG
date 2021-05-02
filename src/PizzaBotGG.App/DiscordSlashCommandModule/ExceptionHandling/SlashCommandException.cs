using System;

namespace PizzaBotGG.App.ExceptionHandling
{
	public class SlashCommandException : Exception
	{
		public SlashCommandException(string message) : base(message)
		{

		}
	}
}

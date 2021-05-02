using System.Reflection;

namespace PizzaBotGG.App.DiscordSlashCommandModule
{
	public class SlashCommandContext
	{
		public SlashCommandContext(MethodInfo commandMethod, object[] parameters)
		{
			CommandMethod = commandMethod;
			Parameters = parameters;
		}

		public MethodInfo CommandMethod { get; }
		public object[] Parameters { get; }
	}
}

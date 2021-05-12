using System;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Attributes
{
	public class SlashAliasAttribute : Attribute
	{
		public string Alias { get; }
		public SlashAliasAttribute(string alias)
		{
			this.Alias = alias;

		}
	}
}

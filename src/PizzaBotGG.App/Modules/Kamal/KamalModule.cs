using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using PizzaBotGG.App.DiscordSlashCommandModule;
using PizzaBotGG.App.DiscordSlashCommandModule.Attributes;
using PizzaBotGG.App.Services;

namespace PizzaBotGG.App.Modules.Kamal
{
    [SlashCommandGroup("kamal", "Kamal quotes")]
    public class KamalModule : SlashModule
    { 
        private static List<string> _quotes = new List<string> 
        {
            "Kut spel!",
            "Wat een kut champion!"
        };
        private readonly IRandomService _randomService;

        public KamalModule(IRandomService randomService)
        {
            _randomService = randomService;
        }

        [SlashCommand("random", "get random Kamal quote")]
        public async Task<DiscordEmbed> GetRandomQuote()
        {
            var index = _randomService.Random(0, _quotes.Count);

            var builder = new DiscordEmbedBuilder();

            var now = DateTime.Now;
            var year = now.Year;
            var quote = _quotes[index];

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(Formatter.Bold(quote));
            stringBuilder.AppendLine(Formatter.Italic($"- Kamal {year}"));
            var content = stringBuilder.ToString();
            builder.WithDescription(content);
            var embed = builder.Build();
            return embed;
        }
    }
}
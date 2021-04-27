using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace PizzaBotGG.App.Converters
{
    public class NullableEnumConverter<TEnum> : IArgumentConverter<TEnum>
        where TEnum : struct, Enum
    {
        public async Task<Optional<TEnum>> ConvertAsync(string value, CommandContext context)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Optional.FromNoValue<TEnum>();
            }

            if (!Enum.TryParse(value, true, out TEnum enumValue))
            {
                await context.RespondAsync("Invalid parameter");
                throw new Exception("invalid parameter");
            }

            var optionalValue = Optional.FromValue(enumValue);

            return optionalValue;
        }
    }
}

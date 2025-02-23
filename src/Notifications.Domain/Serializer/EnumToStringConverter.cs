using Newtonsoft.Json;

namespace Notifications.Domain.Serializer;

public class EnumToStringConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType.IsEnum;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }
        writer.WriteValue(value.ToString());  // Write the enum as its string value
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.Value == null)
        {
            return null;
        }

        if (reader.Value is string enumString)
        {
            try
            {
                return Enum.Parse(objectType, enumString, true); // Case-insensitive parsing
            }
            catch (ArgumentException)
            {
                throw new JsonSerializationException($"Error converting value {enumString} to type '{objectType}'.");
            }
        }

        throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing enum.");
    }
}

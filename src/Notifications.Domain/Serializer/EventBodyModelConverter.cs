using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notifications.Domain.Enums;
using Notifications.Domain.Models.Event;

namespace Notifications.Domain.Serializer;

public class EventBodyModelConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(EventBodyModel).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);

        // Căutăm proprietatea "notificationTypeTag" cu verificare case-insensitive
        if (!jsonObject.TryGetValue("notificationTypeTag", StringComparison.OrdinalIgnoreCase, out JToken tagToken))
        {
            throw new JsonException("Proprietatea notificationTypeTag lipsește.");
        }

        NotificationTypeTagEnum notificationType = tagToken.ToObject<NotificationTypeTagEnum>();

        Type targetType = notificationType switch
        {
            NotificationTypeTagEnum.SUPPLIERS_PORTAL_ACTIVATE_ACCOUNT => typeof(ActivateAccountModel),
            // Adaugă aici alte cazuri pentru alte tipuri
            _ => throw new JsonException($"Tipul de notificare necunoscut: {notificationType}")
        };

        // Elimină temporar acest converter din lista serializer-ului pentru a evita recursivitatea
        var converter = this;
        serializer.Converters.Remove(converter);
        try
        {
            return jsonObject.ToObject(targetType, serializer);
        }
        finally
        {
            serializer.Converters.Add(converter);
        }
    }
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        // Serializare standard folosind JObject
        JObject jo = JObject.FromObject(value, serializer);
        jo.WriteTo(writer);
    }
}

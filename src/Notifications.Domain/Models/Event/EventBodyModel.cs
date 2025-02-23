using System.ComponentModel.DataAnnotations;
using JsonSubTypes;
using Newtonsoft.Json;
using Notifications.Domain.Enums;

namespace Notifications.Domain.Models.Event;

[JsonConverter(typeof(JsonSubtypes), "NotificationTypeTag")]
[JsonSubtypes.KnownSubType(typeof(ActivateAccountModel), NotificationTypeTagEnum.SUPPLIERS_PORTAL_ACTIVATE_ACCOUNT)]
public class EventBodyModel
{
    protected NotificationTypeTagEnum notificationTypeTag;

    [JsonProperty("notificationTypeTag", Required = Required.Always)]
    [Required(ErrorMessage = "Notification type tag should not be null.")]
    public NotificationTypeTagEnum NotificationTypeTag
    {
        get => notificationTypeTag;
        set => notificationTypeTag = value;
    }
}

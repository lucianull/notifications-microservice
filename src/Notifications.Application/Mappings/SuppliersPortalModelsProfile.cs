using AutoMapper;

namespace Notifications.Application.Mappings;

public class SuppliersPortalModelsProfile : Profile
{
    public SuppliersPortalModelsProfile()
    {
        CreateMap<Notifications.Domain.Models.Event.ActivateAccountModel, Notifications.Domain.Models.Notification.ActivateAccountModel>();
    }
}

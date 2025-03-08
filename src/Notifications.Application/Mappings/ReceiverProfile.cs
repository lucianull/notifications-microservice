using AutoMapper;
using Notifications.Domain.Entities;
using Notifications.Domain.Models;

namespace Notifications.Application.Mappings;

public class ReceiverProfile : Profile
{
    public ReceiverProfile()
    {
        CreateMap<ReceiverModel, Receiver>();
    }
}

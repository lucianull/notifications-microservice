using AutoMapper;
using Notifications.Application.Contracts;
using Notifications.Domain.Entities;
using Notifications.Domain.Models;

namespace Notifications.Application.Services;

public class ReceiverService : IReceiverService
{
    private readonly IReceiverRepository _receiverRepository;
    private readonly IMapper _mapper;

    public ReceiverService(
        IReceiverRepository receiverRepository,
        IMapper mapper)
    {
        _receiverRepository = receiverRepository;
        _mapper = mapper;
    }

    public async Task<List<Receiver>> AddReceiversAsync(List<ReceiverModel> receivers)
    {
        var receiverEntities = _mapper.Map<List<Receiver>>(receivers);

        await _receiverRepository.AddReceiversAsync(receiverEntities);

        return receiverEntities;
    }
}

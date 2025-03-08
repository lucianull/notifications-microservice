using System.Linq;
using AutoMapper;
using Notifications.Application.Contracts;
using Notifications.Domain.Entities;
using Notifications.Domain.Enums;
using Notifications.Domain.Exceptions;
using Notifications.Domain.Models;
using Notifications.Domain.Models.Event;
using Notifications.Domain.Models.Notification;
using Notifications.Infrastructure.Repositories;
using Raven.Client.Exceptions;

namespace Notifications.Application.Strategies.NotificationStrategies
{
    public class SuppliersPortalActivateAccountStrategy : INotificationStrategy
    {
        public static NotificationTypeTagEnum Type => NotificationTypeTagEnum.SUPPLIERS_PORTAL_ACTIVATE_ACCOUNT;
        private readonly INotificationTypeRepository _notificationTypeRepository;
        private readonly IReceiverRepository _receiverRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IReceiverService _receiverService;
        private readonly IMapper _mapper;

        public SuppliersPortalActivateAccountStrategy(
            INotificationTypeRepository notificationTypeRepository,
            IReceiverRepository receiverRepository,
            IGroupRepository groupRepository,
            INotificationRepository notificationRepository,
            IReceiverService receiverService,
            IMapper mapper
        )
        {
            _notificationTypeRepository = notificationTypeRepository ?? throw new ArgumentNullException(nameof(notificationTypeRepository));
            _receiverRepository = receiverRepository ?? throw new ArgumentNullException(nameof(receiverRepository));
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _receiverService = receiverService ?? throw new ArgumentNullException(nameof(receiverService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task createEventAsync(EventCreateModel eventCreateModel)
        {
            NotificationType notificationType = await _notificationTypeRepository.GetNotificationTypeByTagAsync(eventCreateModel.Data.NotificationTypeTag);
            var receivers = eventCreateModel.Receivers;
            var emailPhonePairs = receivers.Select(r => new EmailPhonePair { Email = r.Email, Phone = r.Phone }).ToList();
            var existingReceivers = await _receiverRepository.GetReceiversByEmailsAndPhonesAsync(emailPhonePairs);
            var newReceiverModels = receivers
                .Where(r => !existingReceivers.Any(er => er.Email == r.Email && er.Phone == r.Phone))
                .ToList();

            // Create new receivers for those that don't exist
            // If new receivers are created, then the group is also new for sure
            Group? group = null;
            if (newReceiverModels.Any())
            {
                var addedReceivers = await _receiverService.AddReceiversAsync(newReceiverModels);
                existingReceivers = existingReceivers.Concat(addedReceivers).ToList();
                var receiverIds = existingReceivers.Select(r => r.Id).Where(id => id != null).Cast<string>().ToList();
                group = new Group
                {
                    ReceiverIds = receiverIds
                };
                await _groupRepository.AddGroupAsync(group);

            }
            else
            {
                var receiverIds = existingReceivers.Select(r => r.Id).Where(id => id != null).Cast<string>().ToList();
                group = await _groupRepository.GetGroupByReceiverIdsAsync(receiverIds);
                if (group == null)
                {
                    group = new Group
                    {
                        ReceiverIds = receiverIds
                    };
                    await _groupRepository.AddGroupAsync(group);
                }
            }

            Event @event = new Event
            {
                Data = eventCreateModel.Data,
            };
            Notification notification = new Notification
            {
                GroupId = group.Id,
                NotificationTypeId = notificationType.Id,
                EventsCount = 1,
                Events = new List<Event> { @event }
            };
            if (notificationType.TimeFrame > 0 && notificationType.Threshold > 1)
            {
                try
                {
                    await _notificationRepository.TryCreateNotificationAsyncWithLock(notification);
                }
                catch (ResourceConflictException exception)
                {
                    for (int i = 1; i <= Notification.OPTIMISTIC_RETRIES; i++)
                    {
                        try
                        {
                            Notification existingNotification = await _notificationRepository.GetByIdAsync(exception.UniqueKey);
                            existingNotification.AddOrUpdateEvent(@event);
                            await _notificationRepository.SaveAsync(existingNotification);
                            break;
                        }
                        catch (ConcurrencyException ex)
                        {
                            if (i == Notification.OPTIMISTIC_RETRIES)
                            {
                                throw new Exception("Failed to update existing notification after multiple retries.", ex);
                            }
                        }
                    }
                }
            }
            else
            {
                await _notificationRepository.CreateNotificationAsync(notification);
            }

        }

        public Task<NotificationBodyModel> createNotificationBodyObjectAsync(Notification notification)
        {
            throw new NotImplementedException();
        }
    }
}
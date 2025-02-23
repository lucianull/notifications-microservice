using AutoMapper;
using Notifications.Application.Contracts;
using Notifications.Domain.Entities;
using Notifications.Domain.Enums;
using Notifications.Domain.Exceptions;
using Notifications.Domain.Models;
using Notifications.Domain.Models.Event;
using Notifications.Domain.Models.Notification;
using Notifications.Infrastructure.Repositories;

namespace Notifications.Application.Strategies.NotificationStrategies
{
    public class SuppliersPortalActivateAccountStrategy : INotificationStrategy
    {
        public static NotificationTypeTagEnum Type => NotificationTypeTagEnum.SUPPLIERS_PORTAL_ACTIVATE_ACCOUNT;
        private readonly INotificationTypeRepository _notificationTypeRepository;
        private readonly IReceiverRepository _receiverRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public SuppliersPortalActivateAccountStrategy(
            INotificationTypeRepository notificationTypeRepository,
            IReceiverRepository receiverRepository,
            IGroupRepository groupRepository,
            INotificationRepository notificationRepository,
            IMapper mapper
        )
        {
            _notificationTypeRepository = notificationTypeRepository ?? throw new ArgumentNullException(nameof(notificationTypeRepository));
            _receiverRepository = receiverRepository ?? throw new ArgumentNullException(nameof(receiverRepository));
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task createEventAsync(EventCreateModel eventCreateModel)
        {
            NotificationType notificationType = await _notificationTypeRepository.GetNotificationTypeByTagAsync(eventCreateModel.Data.NotificationTypeTag);
            var receiverEmails = eventCreateModel.ReceiverEmails;
            var existingReceivers = await _receiverRepository.GetReceiversByEmailsAsync(eventCreateModel.ReceiverEmails);
            var newReceiverEmails = receiverEmails
                .Except(existingReceivers.Select(r => r.Email))
                .ToList();

            // Create new receivers for those emails
            var newReceivers = newReceiverEmails.Select(email => new Receiver { Email = email }).ToList();
            if(newReceivers.Any())
            {
                await _receiverRepository.AddReceiversAsync(newReceivers);
            }

            var allReceivers = existingReceivers.Concat(newReceivers);


            var receiverIds = allReceivers.Select(r => r.Id).Where(id => id != null).Cast<string>().ToList();
            var group = await _groupRepository.GetGroupByReceiverIdsAsync(receiverIds);

            if (group == null)
            {
                // Handle case where no group was found or create a new one
                group = new Group
                {
                    ReceiverIds = receiverIds
                };
                await _groupRepository.AddGroupAsync(group);
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
            try {
                await _notificationRepository.TryCreateNotificationAsync(notification);
            } catch(ResourceConflictException exception)
            {
                Console.WriteLine($"Notification with id {exception.UniqueKey} already exists. Adding event to existing notification.");
                Notification existingNotification = await _notificationRepository.GetByIdAsync(exception.UniqueKey);
                existingNotification.AddOrUpdateEvent(@event);
                await _notificationRepository.SaveAsync(existingNotification);
            }
        }

        public Task<NotificationBodyModel> createNotificationBodyObjectAsync(Notification notification)
        {
            throw new NotImplementedException();
        }
    }
}
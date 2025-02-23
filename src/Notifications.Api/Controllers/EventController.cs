using Microsoft.AspNetCore.Mvc;
using Notifications.Application.Contracts;
using Notifications.Domain.Models.Event;

namespace Notifications.Api.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventController : ControllerBase
    {
        private readonly INotificationStrategyContext _notificationStrategyContext;

        public EventController(INotificationStrategyContext notificationStrategyContext)
        {
            _notificationStrategyContext = notificationStrategyContext ?? throw new ArgumentNullException(nameof(notificationStrategyContext));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateModel eventCreateModel)
        {
            await _notificationStrategyContext.createEventAsync(eventCreateModel);
            return Ok("Event created");
        }
    }
}
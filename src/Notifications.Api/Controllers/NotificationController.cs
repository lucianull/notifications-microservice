using Microsoft.AspNetCore.Mvc;
using Notifications.Domain.Entities;
using Raven.Client.Documents;
using System.Threading.Tasks;

namespace Notifications.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IDocumentStore _documentStore;

        public NotificationController(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        // POST: api/notification
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
        {
            if (notification == null)
            {
                return BadRequest("Notification cannot be null.");
            }

            using (var session = _documentStore.OpenAsyncSession())
            {
                // Store the new notification in RavenDB
                await session.StoreAsync(notification);
                await session.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetNotification), new { id = notification.Id }, notification);
        }

        // For simplicity, let's assume we have a GetNotification endpoint too
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotification(string id)
        {
            using (var session = _documentStore.OpenAsyncSession())
            {
                var notification = await session.LoadAsync<Notification>(id);
                if (notification == null)
                {
                    return NotFound();
                }

                return Ok(notification);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using mC_Connect_table.Hubs;

using System.Text.Json; // Add this using directive
using System.Threading.Tasks;

namespace mC_Connect_table.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : Controller
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public WebhookController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WebhookRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request");
            }

            // Send notification to all connected clients
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", request);

            // You can also send notifications to specific groups or clients
            // await _hubContext.Clients.Group("someGroup").SendAsync("ReceiveNotification", request);

            return Ok();
        }
    }

    public class WebhookRequest
    {
        public string title { get; set; }
        public int id { get; set; }
    }
}

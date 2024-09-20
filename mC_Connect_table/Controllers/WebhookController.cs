using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using mC_Connect_table.Hubs;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using mC_Connect_table.Models;

namespace mC_Connect_table.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : Controller
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly HttpClient _httpClient;

        // Base64 strings for each of the 4 images
        private readonly string base64ImageAction1 = "data:image/png;base64,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";
        private readonly string base64ImageAction2 = "data:image/png;base64,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";
        private readonly string base64ImageAction3 = "data:image/png;base64,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";
        private readonly string base64ImageAction129 = "data:image/png;base64,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";

        public WebhookController(IHubContext<NotificationHub> hubContext, HttpClient httpClient)
        {
            _hubContext = hubContext;
            _httpClient = httpClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NotificationViewModel request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request");
            }

            // Send notification to all connected clients
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", request);

            // Check if the title is "push-switch-on"
            if (request.title == "push-switch-on")
            {
                // Perform different PUT requests based on action value
                switch (request.action)
                {
                    case 1:
                        await SendPutRequestForAction("https://mc-connect-manager.smcs.io/api/v1/update-image", request, base64ImageAction1);
                        break;
                    case 2:
                        await SendPutRequestForAction("https://mc-connect-manager.smcs.io/api/v1/update-image", request, base64ImageAction2);
                        break;
                    case 3:
                        await SendPutRequestForAction("https://mc-connect-manager.smcs.io/api/v1/update-image", request, base64ImageAction3);
                        break;
                    case 129:
                        await SendPutRequestForAction("https://mc-connect-manager.smcs.io/api/v1/update-image", request, base64ImageAction129);
                        break;
                    default:
                        return BadRequest("Unknown action value.");
                }
            }

            return Ok();
        }

        // Updated method to accept Base64-encoded image content as a parameter
        private async Task SendPutRequestForAction(string endpoint, NotificationViewModel request, string base64ImageContent)
        {
            // Create the body for the PUT request
            var putRequestBody = new
            {
                device_ids = new[] { request.id }, // The id from NotificationViewModel as device_id
                led = 2, // Example LED value
                buzzer = new
                {
                    on_time = 150,    // Example values for buzzer
                    off_time = 250,
                    repetitions = 1
                },
                content = base64ImageContent // Set the Base64-encoded image content dynamically
            };

            // Serialize the body to JSON
            var jsonContent = JsonSerializer.Serialize(putRequestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Send the PUT request
            var response = await _httpClient.PutAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                // Handle failure
                throw new HttpRequestException($"Error sending PUT request to {endpoint}: {response.StatusCode}");
            }
        }
    }
}

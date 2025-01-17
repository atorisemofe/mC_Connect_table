using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using mC_Connect_table.Hubs;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using mC_Connect_table.Models;
using mC_Connect_table.Data;
using Microsoft.EntityFrameworkCore;

namespace mC_Connect_table.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : Controller
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly HttpClient _httpClient;

        private readonly RestaurantTablesContext _context;

        public WebhookController(IHubContext<NotificationHub> hubContext, HttpClient httpClient, RestaurantTablesContext context)
        {
            _hubContext = hubContext;
            _httpClient = httpClient;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NotificationViewModel request)
        {
            var instructionsBase64 = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAZAAAAEsCAAAAADI3LoeAAAEqGlUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSfvu78nIGlkPSdXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQnPz4KPHg6eG1wbWV0YSB4bWxuczp4PSdhZG9iZTpuczptZXRhLyc+CjxyZGY6UkRGIHhtbG5zOnJkZj0naHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyc+CgogPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9JycKICB4bWxuczpBdHRyaWI9J2h0dHA6Ly9ucy5hdHRyaWJ1dGlvbi5jb20vYWRzLzEuMC8nPgogIDxBdHRyaWI6QWRzPgogICA8cmRmOlNlcT4KICAgIDxyZGY6bGkgcmRmOnBhcnNlVHlwZT0nUmVzb3VyY2UnPgogICAgIDxBdHRyaWI6Q3JlYXRlZD4yMDI1LTAxLTE0PC9BdHRyaWI6Q3JlYXRlZD4KICAgICA8QXR0cmliOkV4dElkPmQzYTBlYTU0LTMzNjktNGRkOS05MmEwLWQ4ZjczZGExYjlmNjwvQXR0cmliOkV4dElkPgogICAgIDxBdHRyaWI6RmJJZD41MjUyNjU5MTQxNzk1ODA8L0F0dHJpYjpGYklkPgogICAgIDxBdHRyaWI6VG91Y2hUeXBlPjI8L0F0dHJpYjpUb3VjaFR5cGU+CiAgICA8L3JkZjpsaT4KICAgPC9yZGY6U2VxPgogIDwvQXR0cmliOkFkcz4KIDwvcmRmOkRlc2NyaXB0aW9uPgoKIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PScnCiAgeG1sbnM6ZGM9J2h0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvJz4KICA8ZGM6dGl0bGU+CiAgIDxyZGY6QWx0PgogICAgPHJkZjpsaSB4bWw6bGFuZz0neC1kZWZhdWx0Jz5BZGQgYSBoZWFkaW5nIC0gSG93VG9Vc2U8L3JkZjpsaT4KICAgPC9yZGY6QWx0PgogIDwvZGM6dGl0bGU+CiA8L3JkZjpEZXNjcmlwdGlvbj4KCiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0nJwog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";
            if (request == null)
            {
                return BadRequest("Invalid request");
            }

            // Check if the title is "push-switch-on"
            if (request.title == "push-switch-on")
            {
                // Perform different actions based on the request action value
                switch (request.action)
                {
                    case 1:
                        // Example: Send some notification
                        await _hubContext.Clients.All.SendAsync("ReceiveNotification", request);
                        break;
                        
                    case 2:
                        // Find the table based on request.id and MctId
                        var matchingTable = await _context.RestaurantTables
                            .FirstOrDefaultAsync(t => t.MctId == request.id);

                        if (matchingTable != null)
                        {
                            // Dynamically generate the base URL
                            var baseUrl = $"{Request.Scheme}://{Request.Host}";
                            // Create the URL using the table number
                            string menuUrl = $"https://localhost:7151/Menu/Index?table={matchingTable.TableNumber}&mctid={matchingTable.MctId}";
                            string menuUrl1 = $"{baseUrl}/Menu/Index?table={matchingTable.TableNumber}&mctid={matchingTable.MctId}";

                            await SendQRImage2Click("https://mc-connect-manager.smcs.io/api/v1/update-image", request, menuUrl1, 2);
                            
                            // Wait for 30 seconds (30000 milliseconds)
                            await Task.Delay(30000);

                            await sendInstuctions("https://mc-connect-manager.smcs.io/api/v1/update-image", request, instructionsBase64, 0);
                           
                        }
                        else
                        {
                            return NotFound($"No table found for MctId {request.id}");
                        }
                        break;

                    case 3:
                        await _hubContext.Clients.All.SendAsync("ReceiveNotification", request);
                        break;

                    // case 129:
                    //     await SendPutRequestForAction("https://mc-connect-manager.smcs.io/api/v1/update-image", request, base64ImageAction129, 129);
                    //     break;

                    default:
                        return BadRequest("Unknown action value.");
                }
            }

            return Ok();
        }

        // Updated method to accept Base64-encoded image content as a parameter
        private async Task sendInstuctions(string endpoint, NotificationViewModel request, string base64ImageContent, int repitition)
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
                    repetitions = repitition
                },
                content = base64ImageContent // Set the Base64-encoded image content dynamically
            };

            // Serialize the body to JSON
            var jsonContent = JsonSerializer.Serialize(putRequestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Create an HttpRequestMessage to include the custom header
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, endpoint)
            {
                Content = content
            };

            // Add the custom header
            requestMessage.Headers.Add("Star-Api-Key", "00a7d7db-37e9-4737-8400-5c778d6cc05c");

            // Send the PUT request with the custom header
            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                // Handle failure
                throw new HttpRequestException($"Error sending PUT request to {endpoint}: {response.StatusCode}");
            }
        }

        private async Task SendQRImage2Click(string endpoint, NotificationViewModel request, string qrURL, int repitition)
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
                    repetitions = repitition // Ensure 'repitition' variable is defined elsewhere in your code
                },
                template = new
                {
                    id = "300",
                    merge_data = new object[] 
                    {
                        new 
                        {
                            data = "Scan QR Code to Order",
                            align = 0,
                            font_family = 0,
                            data_type = 0
                        },
                        new
                        {
                            data = qrURL, // Ensure 'qrURL' is defined elsewhere in your code
                            data_type = 1
                        }
                    }
                }
            };

            // Serialize the body to JSON
            var jsonContent = JsonSerializer.Serialize(putRequestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Create an HttpRequestMessage to include the custom header
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, endpoint)
            {
                Content = content
            };

            // Add the custom header
            requestMessage.Headers.Add("Star-Api-Key", "00a7d7db-37e9-4737-8400-5c778d6cc05c");

            // Send the PUT request with the custom header
            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                // Handle failure
                throw new HttpRequestException($"Error sending PUT request to {endpoint}: {response.StatusCode}");
            }
        }

    }
}
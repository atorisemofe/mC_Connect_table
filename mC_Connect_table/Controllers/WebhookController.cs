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
        private async Task SendPutRequestForAction(string endpoint, NotificationViewModel request, string base64ImageContent, int repitition)
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
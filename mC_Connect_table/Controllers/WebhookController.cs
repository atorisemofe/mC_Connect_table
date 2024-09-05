using Microsoft.AspNetCore.Mvc;
using System.Text.Json; // Add this using directive
using System.Threading.Tasks;

namespace mC_Connect_table.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WebhookRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request");
            }

            // Store the data in TempData (convert to JSON string)
            //TempData["NotificationData"] = JsonSerializer.Serialize(request);
            HttpContext.Session.SetString("NotificationData", JsonSerializer.Serialize(request));

            // Redirect to the Notifications controller
            //return RedirectToAction("Index", "Notification");
            return Redirect("/notification/index");
        }
    }

    public class WebhookRequest
    {
        public string title { get; set; }
        public int id { get; set; }
    }
}

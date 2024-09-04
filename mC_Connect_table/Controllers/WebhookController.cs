using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class WebhookController : Controller
{
    // This action listens for incoming webhook notifications
    [HttpPost]
    [Route("api/webhook")]
    public async Task<IActionResult> HandleWebhook([FromBody] WebhookNotification notification)
    {
        // Process the notification based on the title
       // Save notification to database or process as needed
            // For simplicity, let's store it in TempData
            TempData["NotificationTitle"] = notification.title;
            TempData["NotificationId"] = notification.id;
        

        return RedirectToAction("Index","Notifications"); // Respond to the webhook provider that the notification was received
    }
}

// Model to represent the webhook notification payload
public class WebhookNotification
{
    public string title { get; set; }
    public string id { get; set; }
    public string address { get; set; }
    public string device_name { get; set; }
    public string action { get; set; }
    public string job_id { get; set; }
    public string name { get; set; }
    public string intentional { get; set; }
    public string battery_level { get; set; }
}

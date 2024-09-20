using mC_Connect_table.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json; // Add this using directive

namespace mC_Connect_table.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            // Retrieve the data from TempData
            //var notificationData = TempData["NotificationData"] as string;
            var notificationData = HttpContext.Session.GetString("NotificationData");
            
            // Deserialize the data to a model
            var model = notificationData != null 
                ? JsonSerializer.Deserialize<NotificationViewModel>(notificationData) 
                : new NotificationViewModel();

            return View(model);
        }
    }
}

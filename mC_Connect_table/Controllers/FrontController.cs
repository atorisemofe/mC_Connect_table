using mC_Connect_table.Data;
using mC_Connect_table.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace mC_Connect_table.Controllers
{
    public class FrontController : Controller
    {
        private readonly RestaurantTablesContext _context;

        public FrontController(RestaurantTablesContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Retrieve the notification data from the session
            var notificationData = HttpContext.Session.GetString("NotificationData");

            // Fetch the list of tables from the database
            var tables = _context.RestaurantTables.ToList();

            // Deserialize the notification data, if available
            var notification = notificationData != null 
                ? JsonSerializer.Deserialize<NotificationViewModel>(notificationData) 
                : new NotificationViewModel();

            // Create and populate the view model
            var viewModel = new FrontViewModel
            {
                Notification = notification,
                Tables = tables
            };

            // Pass the view model to the view
            return View(viewModel);
        }
    }
}

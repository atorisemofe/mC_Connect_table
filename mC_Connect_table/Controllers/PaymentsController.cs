using mC_Connect_table.Data;
using mC_Connect_table.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace mC_Connect_table.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly OrderViewContext _orderContext;

        public PaymentsController(OrderViewContext orderContext)
        {
            _orderContext = orderContext;
        }

        // Payments page: Shows the list of orders based on sessionID
        public async Task<IActionResult> Index(int table, int mctid, string sessionid)
        {
            // Get all orders for the given sessionID from the database
            var orders = await _orderContext.OrderViewModel
                                 .Where(o => o.SessionId == sessionid && o.PaymentStatus == false)
                                 .ToListAsync();

            // Pass the orders to the view
            return View(orders);
        }

        // Payment processing: Sets the PaymentStatus to true for all matching orders
        [HttpPost]
        public async Task<IActionResult> Pay(string sessionid)
        {
            // Find all orders with the given sessionID
            var orders = _orderContext.OrderViewModel
                                 .Where(o => o.SessionId == sessionid )
                                 .ToList();

            if (orders.Any())
            {
                // Set PaymentStatus to true for all orders
                foreach (var order in orders)
                {
                    order.PaymentStatus = true;
                }

                // Save changes to the database
                await _orderContext.SaveChangesAsync();

                // Redirect to a success page or back to the payments page
                return RedirectToAction("PaymentSuccess");
            }

            return View("Error");
        }

        public IActionResult PaymentSuccess()
        {
            return View();
        }
    }
}
    

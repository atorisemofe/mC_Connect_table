using mC_Connect_table.Data;
using mC_Connect_table.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace mC_Connect_table.Controllers
{
    public class BackController : Controller
    {
        private readonly OrderViewContext _orderContext;

        public BackController(OrderViewContext orderContext)
        {
            _orderContext = orderContext;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _orderContext.OrderViewModel.ToListAsync();
            // var orders = new List<OrderViewModel>
            // {
            //     new OrderViewModel 
            //     { 
            //         OrderNumber = 1, 
            //         TableNumber = 3, 
            //         Status = "Preparing",
            //         EntreeItems = new List<string> { "Vegetable Stir Fry", "Grilled Chicken"}
            //     },
            //     new OrderViewModel 
            //     { 
            //         OrderNumber = 2, 
            //         TableNumber = 5, 
            //         Status = "Ready",
            //         EntreeItems = new List<string> { "Vegetable Stri Fry", "Steak" }
            //     },
            //     new OrderViewModel 
            //     { 
            //         OrderNumber = 3, 
            //         TableNumber = 2, 
            //         Status = "New",
            //         EntreeItems = new List<string> { "Steak", "Grilled Chicken" }
            //     }
            // };
    
            // return View(orders);
            return View(orders);
        }

    }
}
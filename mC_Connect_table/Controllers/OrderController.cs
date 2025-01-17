using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mC_Connect_table.Data;
using mC_Connect_table.Models;

namespace mC_Connect_table.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderViewContext _context;

        public OrderController(OrderViewContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderViewModel.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderViewModel = await _context.OrderViewModel
                .FirstOrDefaultAsync(m => m.OrderNumber == id);
            if (orderViewModel == null)
            {
                return NotFound();
            }

            return View(orderViewModel);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            return View();
        }
        
        // POST: Order/Create
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] OrderViewModel order)
        {
            if (ModelState.IsValid)
            {
                // Create new order entity from the ViewModel
                // var newOrder = new OrderViewModel
                // {
                    order.TableNumber = order.TableNumber;
                    order.Status = order.Status;
                    order.EntreeItems = order.EntreeItems;
                    order.DrinkItems = order.DrinkItems;
                    order.DessertItems = order.DessertItems;
                    order.Notes = order.Notes;
                    order.MctId = order.MctId;
                // };
                _context.Add(order);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Order placed successfully." });
            }
            return Json(new { success = false, message = "Invalid order data." });
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([FromBody] OrderViewModel order)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         // Create new order entity from the ViewModel
        //         // var newOrder = new OrderViewModel
        //         // {
        //             order.TableNumber = order.TableNumber;
        //             order.Status = order.Status;
        //             order.EntreeItems = order.EntreeItems;
        //             order.DrinkItems = order.DrinkItems;
        //             order.DessertItems = order.DessertItems;
        //         // };
        //         _context.Add(order);
        //         await _context.SaveChangesAsync();
        //         return Json(new { success = true, message = "Order placed successfully." });
        //     }
        //     return Json(new { success = false, message = "Invalid order data." });
        // }
        // public async Task<IActionResult> Create([Bind("OrderNumber,TableNumber,Status,EntreeItems,DrinkItems,DessertItems")] OrderViewModel order)
        // {
        //     if (order == null)
        //     {
        //         return BadRequest(new { success = false, message = "Order data is missing." });
        //     }

        //     _context.Add(order);
        //     await _context.SaveChangesAsync();
        //     return Json(new { success = true, message = "Order placed successfully." });
        // }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderViewModel = await _context.OrderViewModel.FindAsync(id);
            if (orderViewModel == null)
            {
                return NotFound();
            }
            return View(orderViewModel);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderNumber,TableNumber,Status,EntreeItems,DrinkItems,DessertItems")] OrderViewModel orderViewModel)
        {
            if (id != orderViewModel.OrderNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderViewModelExists(orderViewModel.OrderNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(orderViewModel);
        }

        //POST: Order/UpdateStatus
        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] OrderViewModel order)
        {
            if (order == null || order.OrderNumber == 0 || string.IsNullOrEmpty(order.Status))
            {
                return Json(new { success = false, message = "Invalid order data." });
            }

            // Fetch the existing order from the database by OrderNumber
            var orderInDb = await _context.OrderViewModel.FindAsync(order.OrderNumber);
            if (orderInDb == null)
            {
                return Json(new { success = false, message = "Order not found." });
            }

            // Update only the status
            orderInDb.Status = order.Status;
            _context.Update(orderInDb);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Order status updated successfully." });
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderViewModel = await _context.OrderViewModel
                .FirstOrDefaultAsync(m => m.OrderNumber == id);
            if (orderViewModel == null)
            {
                return NotFound();
            }

            return View(orderViewModel);
        }

        // POST: Order/Delete
        [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderViewModel = await _context.OrderViewModel.FindAsync(id);
            if (orderViewModel != null)
            {
                _context.OrderViewModel.Remove(orderViewModel);

            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Order Deleted successfully." });
        }

        private bool OrderViewModelExists(int id)
        {
            return _context.OrderViewModel.Any(e => e.OrderNumber == id);
        }
    }
}

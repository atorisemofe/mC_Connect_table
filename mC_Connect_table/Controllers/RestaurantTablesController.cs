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
    public class RestaurantTablesController : Controller
    {
        private readonly RestaurantTablesContext _context;

        public RestaurantTablesController(RestaurantTablesContext context)
        {
            _context = context;
        }

        // PUT: /RestaurantTables/SetTableStatus/5
        [HttpPut("/RestaurantTables/SetTableStatus/{id}")]
        public IActionResult SetTableStatus(int id, bool isActive)
        {
            var table = _context.RestaurantTables.FirstOrDefault(t => t.TableNumber == id);

            if (table == null)
            {
                return NotFound();
            }

            // Update the IsActive status of the table
            table.IsActive = isActive;

            // Save changes to the database
            _context.SaveChanges();

            return Ok();
        }

        // GET: RestaurantTables
        public async Task<IActionResult> Index()
        {
            return View(await _context.RestaurantTables.ToListAsync());
        }

        // GET: RestaurantTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantTables = await _context.RestaurantTables
                .FirstOrDefaultAsync(m => m.TableNumber == id);
            if (restaurantTables == null)
            {
                return NotFound();
            }

            return View(restaurantTables);
        }

        // GET: RestaurantTables/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RestaurantTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TableNumber,MctId")] RestaurantTables restaurantTables)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurantTables);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(restaurantTables);
        }

        // GET: RestaurantTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantTables = await _context.RestaurantTables.FindAsync(id);
            if (restaurantTables == null)
            {
                return NotFound();
            }
            return View(restaurantTables);
        }

        // POST: RestaurantTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TableNumber,MctId")] RestaurantTables restaurantTables)
        {
            if (id != restaurantTables.TableNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurantTables);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantTablesExists(restaurantTables.TableNumber))
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
            return View(restaurantTables);
        }

        // GET: RestaurantTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantTables = await _context.RestaurantTables
                .FirstOrDefaultAsync(m => m.TableNumber == id);
            if (restaurantTables == null)
            {
                return NotFound();
            }

            return View(restaurantTables);
        }

        // POST: RestaurantTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurantTables = await _context.RestaurantTables.FindAsync(id);
            if (restaurantTables != null)
            {
                _context.RestaurantTables.Remove(restaurantTables);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantTablesExists(int id)
        {
            return _context.RestaurantTables.Any(e => e.TableNumber == id);
        }
    }
}

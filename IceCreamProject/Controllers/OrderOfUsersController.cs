using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IceCreamProject.Data;
using IceCreamProject.Models;

namespace IceCreamProject.Controllers
{
    public class OrderOfUsersController : Controller
    {
        private readonly IceCreamProjectContext _context;

        public OrderOfUsersController(IceCreamProjectContext context)
        {
            _context = context;
        }

        // GET: OrderOfUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderOfUser.ToListAsync());
        }

        // GET: OrderOfUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderOfUser = await _context.OrderOfUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderOfUser == null)
            {
                return NotFound();
            }

            return View(orderOfUser);
        }

        // GET: OrderOfUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderOfUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Street,House,City,Price,Temperature,Month,Day")] OrderOfUser orderOfUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderOfUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderOfUser);
        }

        // GET: OrderOfUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderOfUser = await _context.OrderOfUser.FindAsync(id);
            if (orderOfUser == null)
            {
                return NotFound();
            }
            return View(orderOfUser);
        }

        // POST: OrderOfUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Street,House,City,Price,Temperature,Month,Day")] OrderOfUser orderOfUser)
        {
            if (id != orderOfUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderOfUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderOfUserExists(orderOfUser.Id))
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
            return View(orderOfUser);
        }

        // GET: OrderOfUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderOfUser = await _context.OrderOfUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderOfUser == null)
            {
                return NotFound();
            }

            return View(orderOfUser);
        }

        // POST: OrderOfUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderOfUser = await _context.OrderOfUser.FindAsync(id);
            _context.OrderOfUser.Remove(orderOfUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderOfUserExists(int id)
        {
            return _context.OrderOfUser.Any(e => e.Id == id);
        }
    }
}

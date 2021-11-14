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
    public class IcecreamTastesController : Controller
    {
        private readonly IceCreamProjectContext _context;

        public IcecreamTastesController(IceCreamProjectContext context)
        {
            _context = context;
        }

        // GET: IcecreamTastes
        public async Task<IActionResult> Index()
        {
            return View(await _context.IcecreamTaste.ToListAsync());
        }

        // GET: IcecreamTastes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var icecreamTaste = await _context.IcecreamTaste
                .FirstOrDefaultAsync(m => m.Id == id);
            if (icecreamTaste == null)
            {
                return NotFound();
            }

            return View(icecreamTaste);
        }

        // GET: IcecreamTastes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IcecreamTastes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImgLocation")] IcecreamTaste icecreamTaste)
        {
            if (ModelState.IsValid)
            {
                _context.Add(icecreamTaste);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(icecreamTaste);
        }

        // GET: IcecreamTastes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var icecreamTaste = await _context.IcecreamTaste.FindAsync(id);
            if (icecreamTaste == null)
            {
                return NotFound();
            }
            return View(icecreamTaste);
        }

        // POST: IcecreamTastes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImgLocation")] IcecreamTaste icecreamTaste)
        {
            if (id != icecreamTaste.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(icecreamTaste);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IcecreamTasteExists(icecreamTaste.Id))
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
            return View(icecreamTaste);
        }

        // GET: IcecreamTastes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var icecreamTaste = await _context.IcecreamTaste
                .FirstOrDefaultAsync(m => m.Id == id);
            if (icecreamTaste == null)
            {
                return NotFound();
            }

            return View(icecreamTaste);
        }

        // POST: IcecreamTastes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var icecreamTaste = await _context.IcecreamTaste.FindAsync(id);
            _context.IcecreamTaste.Remove(icecreamTaste);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IcecreamTasteExists(int id)
        {
            return _context.IcecreamTaste.Any(e => e.Id == id);
        }
    }
}

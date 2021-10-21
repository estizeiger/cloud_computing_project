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
    public class TastesController : Controller
    {
        private readonly IceCreamProjectContext _context;

        public TastesController(IceCreamProjectContext context)
        {
            _context = context;
        }

        // GET: Tastes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Taste.ToListAsync());
        }

        // GET: Taste/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Taste/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImgLocation")] Taste taste)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taste);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taste);
        }

        private bool TasteExists(int id)
        {
            return _context.Taste.Any(e => e.Id == id);
        }
    }
}

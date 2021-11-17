using IceCreamProject.Data;
using IceCreamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projectDesign.Controllers
{
    public class ConsumerController : Controller
    {
        private readonly IceCreamProjectContext _context;//user

        public ConsumerController(IceCreamProjectContext context)
        {
            _context = context;
        }

        // GET: Consumer
        public async Task<IActionResult> Index()
        {
            return View(await _context.IcecreamTaste.ToListAsync());
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Order()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login1([Bind("Id,UserName,Password")] User user)
        {
            bool found = false;
            foreach (var item in _context.User)
            {
                if (item.UserName == user.UserName && item.Password == user.Password)
                {
                    found = true;
                    return View("~/Views/Manager/ManagerHome.cshtml");
                }
            }
            if(!found)
                return View("~/Views/Manager/Oops.cshtml");
            else
                return View("~/Views/Manager/ManagerHome.cshtml");
        }

        public IActionResult Add()
        {
            return View();
        }


    }
}

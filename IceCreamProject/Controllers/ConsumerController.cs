using IceCreamProject.Data;
using IceCreamProject.Models;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Order()
        {
            return View();
        }

        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Login1(string Username, string Password)
        {
            foreach (var item in _context.User)
            {
                if (item.UserName == Username && item.Password == Password)
                {
                    return View("~/Views/Manager/Index.cshtml");
                }
            }
            return View("~/Views/Manager/Index.cshtml");
        }

        public IActionResult Add()
        {
            return View();
        }


    }
}

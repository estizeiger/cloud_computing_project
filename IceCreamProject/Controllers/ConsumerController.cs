using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projectDesign.Controllers
{
    public class ConsumerController : Controller
    {
        private List<string> _context;//list of manager name+ password

        string managerUserName = "dani11";
        string managerPassword = "dindin11";

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
            _context = new List<string>();
            _context.Insert(0, managerUserName);
            _context.Insert(1, managerPassword);

            ViewBag.Message = _context;//list of manager name+ password
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }


    }
}

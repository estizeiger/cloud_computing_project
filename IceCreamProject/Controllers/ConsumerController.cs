using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projectDesign.Controllers
{
    public class ConsumerController : Controller
    {
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
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }


    }
}

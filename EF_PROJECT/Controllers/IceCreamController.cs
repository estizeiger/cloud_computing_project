using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EF_PROJECT.Controllers
{
    public class IceCreamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

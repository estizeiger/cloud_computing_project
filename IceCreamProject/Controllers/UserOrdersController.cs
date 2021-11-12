using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IceCreamProject.Data;
using IceCreamProject.Models;

using RestSharp;
using Newtonsoft.Json;

namespace IceCreamProject.Controllers
{
    public class UserOrdersController : Controller
    {
        private readonly IceCreamProjectContext _context;//order
        private readonly IceCreamProjectContext _context2;//taste

        public UserOrdersController(IceCreamProjectContext context, IceCreamProjectContext context2)
        {
            _context = context;//order
            _context2 = context2;//taste
        }

        // GET: UserOrders
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserOrder.ToListAsync());
        }

        // GET: UserOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userOrder = await _context.UserOrder
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userOrder == null)
            {
                return NotFound();
            }

            return View(userOrder);
        }

        // GET: UserOrders/Create
        public IActionResult Create()
        {
            ViewBag.Message = _context2.Taste;//here we send the taste model, as a message

            return View();
        }

        // POST: UserOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TasteId,UserName,Address,Price,Date,FeelsLike,Pressure,Humidity")] UserOrder userOrder)
        {
            if (ModelState.IsValid)
            {
                var arr = userOrder.Address.Split(',');
                var city = arr[1];
                userOrder.Address = city;

                //enter price and tasteId values from ui :
                //userOrder.TasteId=
                //userOrder.Price=

                //enter weather details
                Main weather = findWeather(userOrder.Address);
                userOrder.FeelsLike = weather.feels_like;
                userOrder.Pressure = weather.pressure;
                userOrder.Humidity = weather.humidity;
                //enter date and hour
                DateTime date = DateTime.Now;
                userOrder.Date = date;

                _context.Add(userOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userOrder);
        }

        // GET: UserOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userOrder = await _context.UserOrder.FindAsync(id);
            if (userOrder == null)
            {
                return NotFound();
            }
            return View(userOrder);
        }

        // POST: UserOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TasteId,UserName,Address,Price,Date,FeelsLike,Pressure,Humidity")] UserOrder userOrder)
        {
            if (id != userOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserOrderExists(userOrder.Id))
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
            return View(userOrder);
        }

        // GET: UserOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userOrder = await _context.UserOrder
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userOrder == null)
            {
                return NotFound();
            }

            return View(userOrder);
        }

        // POST: UserOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userOrder = await _context.UserOrder.FindAsync(id);
            _context.UserOrder.Remove(userOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserOrderExists(int id)
        {
            return _context.UserOrder.Any(e => e.Id == id);
        }

        /****************open weather*********************/
        public Main CheckWeather(string address)
        {
            /*
             * api.openweathermap.org/data/2.5/weather?q={city name}&appid={API key}
             */
            Main result = null;
            string apiKey = "1922cf83f6dd2e31e48c6d17f3309818";
            var client = new RestSharp.RestClient("https://api.openweathermap.org/data/2.5/weather");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddParameter("q", address);
            request.AddParameter("appid", apiKey);
            request.AddParameter("units", "metric");
            IRestResponse response = client.Execute(request);
            result = ConvertToDictionary(response.Content);
            return result;
        }
        public Main ConvertToDictionary(string response)
        {
            Root TheTags = JsonConvert.DeserializeObject<Root>(response);
            return TheTags.main;
        }
        public Main findWeather(string address)
        {
            Weather weather = new Weather();
            Main result = CheckWeather(address);
            return result;
        }


    }
}

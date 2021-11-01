//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using IceCreamProject.Data;
//using IceCreamProject.Models;

//using RestSharp;
//using Newtonsoft.Json;

//namespace IceCreamProject.Controllers
//{
//    public class OrderDetailsController : Controller
//    {
//        private readonly IceCreamProjectContext _context;//order
//        private readonly IceCreamProjectContext _context2;//taste

//        public OrderDetailsController(IceCreamProjectContext context, IceCreamProjectContext context2)
//        {
//            _context = context;//order
//            _context2 = context2;//taste
//        }

//        // GET: OrderDetails
//        public async Task<IActionResult> Index()
//        {
//            ViewBag.Message = _context2.Taste;//here we send the taste model, as a message

//            //return View(await _context.OrderDetail.ToListAsync());//send orders model
//            return View();
//        }

//        // GET: OrderDetails/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var orderDetail = await _context.OrderDetail
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (orderDetail == null)
//            {
//                return NotFound();
//            }

//            return View(orderDetail);
//        }

//        // GET: OrderDetails/Create
//        public IActionResult Create()
//        {
//            ViewBag.Message = _context2.Taste;//here we send the taste model, as a message

//            return View();//order model
//        }

//        // POST: OrderDetails/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,TasteId,Street,House,City,Price,Temperature,Month,Day,feelsLike,humidity,pressure")] OrderDetail orderDetail)
//        {
//            if (ModelState.IsValid)
//            {
//                var arr = orderDetail.City.Split(',');
//                var city = arr[1];
//                orderDetail.City = city;
//                orderDetail.feelsLike = 20;
//                orderDetail.humidity = 33;
//                orderDetail.pressure = 20;
//                _context.Add(orderDetail);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(orderDetail);
//        }

//        //public async Task<IActionResult> Create([Bind("Id,Name,PhoneNumber,Email,Street,City,HouseNumber,IceCream,Date,FeelsLike,Humidity,Pressure")] Order order)
//        //{
//        //    if (ModelState.IsValid)
//        //    {


//        //        //enter weather details
//        //        Main weather = findWeather(order.City);
//        //        order.FeelsLike = weather.feels_like;
//        //        order.Pressure = weather.pressure;
//        //        order.Humidity = weather.humidity;
//        //        //enter date and hour
//        //        DateTime date = DateTime.Now;
//        //        order.Date = date;
//        //        _context.Add(order);
//        //        await _context.SaveChangesAsync();
//        //        return RedirectToAction(nameof(Index));

//        //    }
//        //    return View(order);
//        //}


//        // GET: OrderDetails/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            //var orderDetail = await _context.OrderDetail.FindAsync(id);
//            //if (orderDetail == null)
//            //{
//            //    return NotFound();
//            //}
//            return View(/*orderDetail*/);
//        }

//        // POST: OrderDetails/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,TasteId,Street,House,City,Price,Temperature,Month,Day")] OrderDetail orderDetail)
//        {
//            if (id != orderDetail.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(orderDetail);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!OrderDetailExists(orderDetail.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(orderDetail);
//        }

//        // GET: OrderDetails/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var orderDetail = await _context.OrderDetail
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (orderDetail == null)
//            {
//                return NotFound();
//            }

//            return View(orderDetail);
//        }

//        // POST: OrderDetails/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var orderDetail = await _context.OrderDetail.FindAsync(id);
//            _context.OrderDetail.Remove(orderDetail);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool OrderDetailExists(int id)
//        {
//            return _context.OrderDetail.Any(e => e.Id == id);
//        }

//        /*************open weather*************/
//        public Main CheckWeather(string city)
//        {
//            /*
//             * api.openweathermap.org/data/2.5/weather?q={city name}&appid={API key}
//             */
//            Main result = null;
//            string apiKey = "1922cf83f6dd2e31e48c6d17f3309818";
//            var client = new RestSharp.RestClient("https://api.openweathermap.org/data/2.5/weather");
//            client.Timeout = -1;
//            var request = new RestRequest(Method.GET);
//            request.AddParameter("q", city);
//            request.AddParameter("appid", apiKey);
//            request.AddParameter("units", "metric");
//            IRestResponse response = client.Execute(request);
//            result = ConvertToDictionary(response.Content);
//            return result;
//        }
//        public Main ConvertToDictionary(string response)
//        {
//            Root TheTags = JsonConvert.DeserializeObject<Root>(response);
//            return TheTags.main;
//        }
//        public Main findWeather(string city)
//        {
//            Weather weather = new Weather();
//            Main result = CheckWeather(city);
//            return result;
//        }


//    }
//}

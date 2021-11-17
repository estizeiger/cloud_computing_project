using IceCreamProject.Data;
using IceCreamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System.IO;

using System.Threading;
using Firebase.Auth;
using Firebase.Storage;
using RestSharp;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace projectDesign.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IceCreamProjectContext _context;
        private double price;

        private readonly IWebHostEnvironment _env;

        private static int _tasteId;

        private bool notIceCream;

        // Configure Firebase
        private static string ApiKey = "AIzaSyD715f3Vj0QgK6FuKqcx9Wn6kQcGAAjBJE";
        private static string Bucket = "cloud-computing-2fb12.appspot.com";
        private static string AuthEmail = "cloudComputingTestUser@gmail.com";
        private static string AuthPassword = "123456";

        public ManagerController(IceCreamProjectContext context,IWebHostEnvironment env)
        {
            _context = context;
            price = 18;
            _env = env;

            notIceCream = false;
        }

        public IActionResult ManagerHome()
        {
            return View();
        }

        public IActionResult Oops()
        {
            return View("~/Views/Manager/Oops.cshtml");
        }

        // GET: Manager/Menu
        public async Task<IActionResult> Menu()
        {
            return View(await _context.IcecreamTaste.ToListAsync());
        }

        public bool CheckImg(string ImageUrl)
        {
            string apiKey = "acc_58df1a604b996df";
            string apiSecret = "c3ae775e2bffa0ab99e7db1dbef3bf6d";

            string basicAuthValue = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", apiKey, apiSecret)));

            var client = new RestClient("https://api.imagga.com/v2/tags");
            client.Timeout = -1;
            //var _url = "https://docs.imagga.com/static/images/docs/sample/japan-605234_1280.jpg";
            var request = new RestRequest(Method.GET);
            request.AddParameter("image_url", ImageUrl);
            request.AddHeader("Authorization", String.Format("Basic {0}", basicAuthValue));
            IRestResponse response = client.Execute(request);
            bool validPic= ConvertToDict(response.Content);
            return validPic;
        }
        /*
         *         public List<string> CheckImage(string ImageUrl)
        {
            List<string> Result =null;
            string apiKey = "acc_c4ce731bb14dcf7";
            string apiSecret = "8d833fd35787d0450fc558d13451d0f2";
           // string imageUrl = "https://d1ymz67w5raq8g.cloudfront.net/Pictures/780xany/3/4/2/513342_gettyimages1053776630_690692.jpg";

            string basicAuthValue = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", apiKey, apiSecret)));

            var client = new RestClient("https://api.imagga.com/v2/tags");
            client.Timeout = -1;

            var request = new RestRequest(Method.GET);
            request.AddParameter("image_url", ImageUrl);
            request.AddHeader("Authorization", String.Format("Basic {0}", basicAuthValue));

            IRestResponse response = client.Execute(request);
            // Console.Write(response.Content);
            Result = ConvertToList(response.Content);
            return Result;
        }

        public List<string> ConvertToList(string response)
        {
            List<string> Result =new List<string>();

            Root TheTags = JsonConvert.DeserializeObject<Root>(response);


            foreach (Tag item in TheTags.result.tags)
            {
                Result.Add(item.tag.en);
            }
    
            return Result;
        }
         * 
         * 
         * 
         * 
         */

        public bool ConvertToDict(string response)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();
            MyResponse.Root TheTags = JsonConvert.DeserializeObject<MyResponse.Root>(response);
            foreach (MyResponse.Tag item in TheTags.result.tags)
            {
                if (item.tag.en == "ice cream" && item.confidence > 85|| item.tag.en == "ice" && item.confidence > 20)
                {
                    return true;
                    //Result.Add(item.confidence.ToString(), item.tag.en);
                }
            }
            return  false;
        }

        // GET: Manager/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Message = price;//send to view

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

        // GET: Manager/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manager/Create
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

                _tasteId = icecreamTaste.Id;

                return RedirectToAction(nameof(Index));
            }
            return View(icecreamTaste);
        }

        // GET: Manager/Edit/5
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

        // POST: Manager/Edit/5
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
                return RedirectToAction(nameof(Menu));
            }
            return View(icecreamTaste);
        }

        // GET: Manager/Delete/5
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

        // POST: Manager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var icecreamTaste = await _context.IcecreamTaste.FindAsync(id);
            _context.IcecreamTaste.Remove(icecreamTaste);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Menu));
        }

        private bool IcecreamTasteExists(int id)
        {
            return _context.IcecreamTaste.Any(e => e.Id == id);
        }

        public IActionResult Analysis()
        {
            return View();
        }


        public IActionResult Index()
        {
            if (notIceCream)
                return View("~/Views/Manager/Oops.cshtml");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(FileUploadModel model)
        {
            var file = model.File;

            string fileName = file.FileName;
            FileStream stream = null;
            if (file.Length > 0)
            {
                string path = Path.Combine(_env.WebRootPath, $"images/icecreams", fileName);

                using (stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                }
                stream.Close();
                
                var ms = new FileStream(path, FileMode.Open);
                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                // you can use CancellationTokenSource to cancel the upload midway
                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                })
                    .Child("receipts")
                    .Child("test")
                    .Child(fileName)
                    .PutAsync(ms, cancellation.Token);


                //task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

                try
                {
                    ViewBag.link = await task;
                    string url = ViewBag.link;
                    if (CheckImg(url))
                    {
                        // Answer for question #1
                        var entity = _context.IcecreamTaste.FirstOrDefault(item => item.Id == _tasteId);

                        // Validate entity is not null
                        if (entity != null)
                        {
                            // Answer for question #2
                            // Make changes on entity
                            entity.ImgLocation = url;
                            // Save changes in database
                            //await _context.SaveChangesAsync();
                            _context.SaveChanges();

                        }
                    }
                    else
                    {
                        //delete the icecreamTaste that was added:
                        var addedIce = _context.IcecreamTaste.FirstOrDefault(m => m.Id == _tasteId);
                        _context.IcecreamTaste.Remove(addedIce);
                        //await _context.SaveChangesAsync();
                        _context.SaveChanges();

                        //TextWrite("The image apploaded was not of an icecream!");
                        //ModelState.AddModelError(nameof(IcecreamTaste.ImgLocation), "The image apploaded was not of an icecream! Please provide an ice-cream image");

                        ViewData["ImageError"] = true;

                        notIceCream = true;
                        //return View("~/Views/Manager/Oops.cshtml");
                        return RedirectToAction(nameof(Oops));
                    }

                    return Ok();
                    //return null;
                }
                catch (Exception ex)
                {
                    ViewBag.error = $"Exception was thrown: {ex}";
                }
            }

            return BadRequest();
            //return null;
        }

        public ViewResult TextWrite(string ans)
        {
            ViewBag.text = ans;
            return View("~/Manager/Index.cshtml");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /**************************prediction***********************/
        // GET: Manager/Prediction
        public IActionResult Prediction()
        {
            return View();
        }

        // POST: Manager/Prediction
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult Prediction([Bind("Id,TasteId,UserName,Address,Price,Date,FeelsLike,Pressure,Humidity")] UserOrder userFictionalOrder)
        {
            bool correctAddress = true;
            if (ModelState.IsValid)
            {
                //find all needed fields:
                var regex = new Regex("^[.*\\sa-zA-Z|]+$");
                var arr = userFictionalOrder.Address.Split(',');
                foreach (var item in arr)
                {
                    if (!regex.IsMatch(item))//if address in english letters
                    {
                        correctAddress = false;
                    }
                }
                if (correctAddress)
                {
                    var city = arr[1];
                    userFictionalOrder.Address = city;

                    //enter weather details
                    Main weather = findWeather(userFictionalOrder.Address);
                    userFictionalOrder.FeelsLike = weather.feels_like;
                    userFictionalOrder.Pressure = weather.pressure;
                    userFictionalOrder.Humidity = weather.humidity;

                    //enter date and hour
                    DateTime date = DateTime.Now;
                    userFictionalOrder.Date = date;
                    int day = (int)date.DayOfWeek;

                    //predict:
                    double ans;
                    ans = PredictTasteid(userFictionalOrder.UserName, userFictionalOrder.FeelsLike, userFictionalOrder.Pressure, userFictionalOrder.Humidity, day, userFictionalOrder.Date.Day, userFictionalOrder.Date.Month);
                    int tasteId = (int)Math.Round(ans);

                    String tasteName = "example";
                    foreach (var item in _context.IcecreamTaste)
                    {
                        if (item.Id == tasteId)
                        {
                            tasteName = item.Name;
                        }
                    }
                    ViewBag.text = tasteName;
                }
                else//wrong address formate
                {
                    ViewBag.text = "Cannot Predict- Wrong Address format!";
                    //return View("~/Views/Manager/Oops.cshtml");
                }
                    


                return View();
            }
            ViewBag.text = "";
            return View();
        }

        /**
        *  Predictor for TasteId from model/6193e2618be2aa39c000392a
        *  Predictive model by BigML - Machine Learning Made Easy
        */

        public double PredictTasteid(string username, double feelslike, double pressure, double humidity, int dateDayOfWeek, int dateDayOfMonth, int dateMonth)
            {
                if (username == null)
                {
                    return 3.13063D;
                }
                if (username.Equals("f"))
                {
                    if (humidity == 0)
                    {
                        return 2.9026D;
                    }
                    if (humidity > 65)
                    {
                        if (pressure == 0)
                        {
                            return 2.58621D;
                        }
                        if (pressure > 905)
                        {
                            if (pressure > 952)
                            {
                                if (pressure > 957)
                                {
                                    if (dateDayOfWeek == 0)
                                    {
                                        return 2.65789D;
                                    }
                                    if (dateDayOfWeek > 2)
                                    {
                                        if (pressure > 1018)
                                        {
                                            if (humidity > 85)
                                            {
                                                if (feelslike == 0)
                                                {
                                                    return 1.8D;
                                                }
                                                if (feelslike > 10.65)
                                                {
                                                    return 2D;
                                                }
                                                if (feelslike <= 10.65)
                                                {
                                                    return 1D;
                                                }
                                            }
                                            if (humidity <= 85)
                                            {
                                                if (pressure > 1043)
                                                {
                                                    if (pressure > 1082)
                                                    {
                                                        return 4D;
                                                    }
                                                    if (pressure <= 1082)
                                                    {
                                                        return 2.75D;
                                                    }
                                                }
                                                if (pressure <= 1043)
                                                {
                                                    return 4.5D;
                                                }
                                            }
                                        }
                                        if (pressure <= 1018)
                                        {
                                            if (pressure > 990)
                                            {
                                                return 1D;
                                            }
                                            if (pressure <= 990)
                                            {
                                                if (pressure > 968)
                                                {
                                                    return 2D;
                                                }
                                                if (pressure <= 968)
                                                {
                                                    return 3D;
                                                }
                                            }
                                        }
                                    }
                                    if (dateDayOfWeek <= 2)
                                    {
                                        if (humidity > 95)
                                        {
                                            return 4.66667D;
                                        }
                                        if (humidity <= 95)
                                        {
                                            if (dateMonth == 0)
                                            {
                                                return 2.78571D;
                                            }
                                            if (dateMonth > 1)
                                            {
                                                if (feelslike == 0)
                                                {
                                                    return 2.61538D;
                                                }
                                                if (feelslike > 14)
                                                {
                                                    if (dateDayOfMonth == 0)
                                                    {
                                                        return 1.6D;
                                                    }
                                                    if (dateDayOfMonth > 9)
                                                    {
                                                        return 2.5D;
                                                    }
                                                    if (dateDayOfMonth <= 9)
                                                    {
                                                        return 1D;
                                                    }
                                                }
                                                if (feelslike <= 14)
                                                {
                                                    if (humidity > 89)
                                                    {
                                                        return 2D;
                                                    }
                                                    if (humidity <= 89)
                                                    {
                                                        if (dateDayOfMonth == 0)
                                                        {
                                                            return 3.66667D;
                                                        }
                                                        if (dateDayOfMonth > 1)
                                                        {
                                                            if (pressure > 1050)
                                                            {
                                                                return 3D;
                                                            }
                                                            if (pressure <= 1050)
                                                            {
                                                                return 4.25D;
                                                            }
                                                        }
                                                        if (dateDayOfMonth <= 1)
                                                        {
                                                            return 2D;
                                                        }
                                                    }
                                                }
                                            }
                                            if (dateMonth <= 1)
                                            {
                                                return 5D;
                                            }
                                        }
                                    }
                                }
                                if (pressure <= 957)
                                {
                                    return 5D;
                                }
                            }
                            if (pressure <= 952)
                            {
                                if (pressure > 928)
                                {
                                    if (pressure > 940)
                                    {
                                        if (pressure > 947)
                                        {
                                            return 1D;
                                        }
                                        if (pressure <= 947)
                                        {
                                            return 2D;
                                        }
                                    }
                                    if (pressure <= 940)
                                    {
                                        return 1D;
                                    }
                                }
                                if (pressure <= 928)
                                {
                                    if (dateMonth == 0)
                                    {
                                        return 2.66667D;
                                    }
                                    if (dateMonth > 1)
                                    {
                                        if (humidity > 77)
                                        {
                                            return 5D;
                                        }
                                        if (humidity <= 77)
                                        {
                                            return 2D;
                                        }
                                    }
                                    if (dateMonth <= 1)
                                    {
                                        return 1D;
                                    }
                                }
                            }
                        }
                        if (pressure <= 905)
                        {
                            return 4.66667D;
                        }
                    }
                    if (humidity <= 65)
                    {
                        if (humidity > 63)
                        {
                            if (pressure == 0)
                            {
                                return 4.2D;
                            }
                            if (pressure > 981)
                            {
                                return 3.66667D;
                            }
                            if (pressure <= 981)
                            {
                                return 5D;
                            }
                        }
                        if (humidity <= 63)
                        {
                            if (feelslike == 0)
                            {
                                return 3.03297D;
                            }
                            if (feelslike > 28.27083)
                            {
                                return 1.66667D;
                            }
                            if (feelslike <= 28.27083)
                            {
                                if (feelslike > 1.125)
                                {
                                    if (pressure == 0)
                                    {
                                        return 3.14634D;
                                    }
                                    if (pressure > 1089)
                                    {
                                        return 4.33333D;
                                    }
                                    if (pressure <= 1089)
                                    {
                                        if (pressure > 1084)
                                        {
                                            return 1.5D;
                                        }
                                        if (pressure <= 1084)
                                        {
                                            if (dateDayOfMonth == 0)
                                            {
                                                return 3.14286D;
                                            }
                                            if (dateDayOfMonth > 2)
                                            {
                                                if (dateMonth == 0)
                                                {
                                                    return 3.04615D;
                                                }
                                                if (dateMonth > 5)
                                                {
                                                    if (humidity > 36)
                                                    {
                                                        if (pressure > 1039)
                                                        {
                                                            return 4.75D;
                                                        }
                                                        if (pressure <= 1039)
                                                        {
                                                            if (dateDayOfWeek == 0)
                                                            {
                                                                return 2.5D;
                                                            }
                                                            if (dateDayOfWeek > 3)
                                                            {
                                                                if (dateDayOfWeek > 6)
                                                                {
                                                                    return 1D;
                                                                }
                                                                if (dateDayOfWeek <= 6)
                                                                {
                                                                    if (pressure > 969)
                                                                    {
                                                                        return 2.25D;
                                                                    }
                                                                    if (pressure <= 969)
                                                                    {
                                                                        if (dateDayOfWeek > 4)
                                                                        {
                                                                            if (pressure > 961)
                                                                            {
                                                                                return 5D;
                                                                            }
                                                                            if (pressure <= 961)
                                                                            {
                                                                                if (humidity > 37)
                                                                                {
                                                                                    return 4D;
                                                                                }
                                                                                if (humidity <= 37)
                                                                                {
                                                                                    return 3D;
                                                                                }
                                                                            }
                                                                        }
                                                                        if (dateDayOfWeek <= 4)
                                                                        {
                                                                            return 2.33333D;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (dateDayOfWeek <= 3)
                                                            {
                                                                if (feelslike > 16.25)
                                                                {
                                                                    return 2.5D;
                                                                }
                                                                if (feelslike <= 16.25)
                                                                {
                                                                    return 1D;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (humidity <= 36)
                                                    {
                                                        if (pressure > 1041)
                                                        {
                                                            if (feelslike > 25.15)
                                                            {
                                                                return 1D;
                                                            }
                                                            if (feelslike <= 25.15)
                                                            {
                                                                if (pressure > 1070)
                                                                {
                                                                    return 4D;
                                                                }
                                                                if (pressure <= 1070)
                                                                {
                                                                    if (dateMonth > 6)
                                                                    {
                                                                        return 3D;
                                                                    }
                                                                    if (dateMonth <= 6)
                                                                    {
                                                                        return 2D;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (pressure <= 1041)
                                                        {
                                                            if (humidity > 20)
                                                            {
                                                                if (pressure > 1009)
                                                                {
                                                                    return 5D;
                                                                }
                                                                if (pressure <= 1009)
                                                                {
                                                                    if (pressure > 933)
                                                                    {
                                                                        if (pressure > 979)
                                                                        {
                                                                            return 4.5D;
                                                                        }
                                                                        if (pressure <= 979)
                                                                        {
                                                                            return 3D;
                                                                        }
                                                                    }
                                                                    if (pressure <= 933)
                                                                    {
                                                                        return 5D;
                                                                    }
                                                                }
                                                            }
                                                            if (humidity <= 20)
                                                            {
                                                                return 2D;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (dateMonth <= 5)
                                                {
                                                    if (feelslike > 20.5)
                                                    {
                                                        return 1D;
                                                    }
                                                    if (feelslike <= 20.5)
                                                    {
                                                        if (dateDayOfWeek == 0)
                                                        {
                                                            return 2.82353D;
                                                        }
                                                        if (dateDayOfWeek > 1)
                                                        {
                                                            if (humidity > 48)
                                                            {
                                                                if (pressure > 953)
                                                                {
                                                                    if (dateDayOfWeek > 4)
                                                                    {
                                                                        return 2D;
                                                                    }
                                                                    if (dateDayOfWeek <= 4)
                                                                    {
                                                                        if (feelslike > 14.15)
                                                                        {
                                                                            return 4D;
                                                                        }
                                                                        if (feelslike <= 14.15)
                                                                        {
                                                                            return 2.66667D;
                                                                        }
                                                                    }
                                                                }
                                                                if (pressure <= 953)
                                                                {
                                                                    return 4D;
                                                                }
                                                            }
                                                            if (humidity <= 48)
                                                            {
                                                                if (pressure > 934)
                                                                {
                                                                    return 1.25D;
                                                                }
                                                                if (pressure <= 934)
                                                                {
                                                                    return 3D;
                                                                }
                                                            }
                                                        }
                                                        if (dateDayOfWeek <= 1)
                                                        {
                                                            return 4.5D;
                                                        }
                                                    }
                                                }
                                            }
                                            if (dateDayOfMonth <= 2)
                                            {
                                                if (pressure > 1043)
                                                {
                                                    return 2.66667D;
                                                }
                                                if (pressure <= 1043)
                                                {
                                                    if (pressure > 918)
                                                    {
                                                        if (pressure > 980)
                                                        {
                                                            return 3.75D;
                                                        }
                                                        if (pressure <= 980)
                                                        {
                                                            return 5D;
                                                        }
                                                    }
                                                    if (pressure <= 918)
                                                    {
                                                        return 3D;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (feelslike <= 1.125)
                                {
                                    if (feelslike > 0.7)
                                    {
                                        return 1D;
                                    }
                                    if (feelslike <= 0.7)
                                    {
                                        return 2.75D;
                                    }
                                }
                            }
                        }
                    }
                }
                if (!username.Equals("f"))
                {
                    if (dateDayOfMonth == 0)
                    {
                        return 3.25172D;
                    }
                    if (dateDayOfMonth > 3)
                    {
                        if (pressure == 0)
                        {
                            return 3.38462D;
                        }
                        if (pressure > 924)
                        {
                            if (pressure > 982)
                            {
                                if (pressure > 1001)
                                {
                                    if (humidity == 0)
                                    {
                                        return 3.42574D;
                                    }
                                    if (humidity > 91)
                                    {
                                        if (dateDayOfMonth > 11)
                                        {
                                            return 3D;
                                        }
                                        if (dateDayOfMonth <= 11)
                                        {
                                            if (dateMonth == 0)
                                            {
                                                return 4.85714D;
                                            }
                                            if (dateMonth > 1)
                                            {
                                                return 5D;
                                            }
                                            if (dateMonth <= 1)
                                            {
                                                return 4D;
                                            }
                                        }
                                    }
                                    if (humidity <= 91)
                                    {
                                        if (humidity > 24)
                                        {
                                            if (dateMonth == 0)
                                            {
                                                return 3.42857D;
                                            }
                                            if (dateMonth > 2)
                                            {
                                                if (dateMonth > 4)
                                                {
                                                    if (pressure > 1067)
                                                    {
                                                        if (dateMonth > 6)
                                                        {
                                                            if (pressure > 1085)
                                                            {
                                                                return 1.33333D;
                                                            }
                                                            if (pressure <= 1085)
                                                            {
                                                                if (dateDayOfMonth > 5)
                                                                {
                                                                    if (pressure > 1070)
                                                                    {
                                                                        return 2D;
                                                                    }
                                                                    if (pressure <= 1070)
                                                                    {
                                                                        return 2.66667D;
                                                                    }
                                                                }
                                                                if (dateDayOfMonth <= 5)
                                                                {
                                                                    return 1D;
                                                                }
                                                            }
                                                        }
                                                        if (dateMonth <= 6)
                                                        {
                                                            if (humidity > 53)
                                                            {
                                                                return 4.75D;
                                                            }
                                                            if (humidity <= 53)
                                                            {
                                                                return 3.66667D;
                                                            }
                                                        }
                                                    }
                                                    if (pressure <= 1067)
                                                    {
                                                        if (pressure > 1042)
                                                        {
                                                            if (feelslike == 0)
                                                            {
                                                                return 4.58333D;
                                                            }
                                                            if (feelslike > 17.6)
                                                            {
                                                                if (username.Equals("g"))
                                                                {
                                                                    return 5D;
                                                                }
                                                                if (!username.Equals("g"))
                                                                {
                                                                    return 3.33333D;
                                                                }
                                                            }
                                                            if (feelslike <= 17.6)
                                                            {
                                                                return 5D;
                                                            }
                                                        }
                                                        if (pressure <= 1042)
                                                        {
                                                            if (username.Equals("g"))
                                                            {
                                                                if (dateDayOfMonth > 9)
                                                                {
                                                                    if (feelslike == 0)
                                                                    {
                                                                        return 2.42857D;
                                                                    }
                                                                    if (feelslike > 23.4)
                                                                    {
                                                                        return 2D;
                                                                    }
                                                                    if (feelslike <= 23.4)
                                                                    {
                                                                        if (dateDayOfWeek == 0)
                                                                        {
                                                                            return 2.75D;
                                                                        }
                                                                        if (dateDayOfWeek > 2)
                                                                        {
                                                                            return 3.5D;
                                                                        }
                                                                        if (dateDayOfWeek <= 2)
                                                                        {
                                                                            return 2D;
                                                                        }
                                                                    }
                                                                }
                                                                if (dateDayOfMonth <= 9)
                                                                {
                                                                    return 4D;
                                                                }
                                                            }
                                                            if (!username.Equals("g"))
                                                            {
                                                                if (dateDayOfMonth > 5)
                                                                {
                                                                    if (dateMonth > 7)
                                                                    {
                                                                        if (dateDayOfMonth > 8)
                                                                        {
                                                                            return 5D;
                                                                        }
                                                                        if (dateDayOfMonth <= 8)
                                                                        {
                                                                            return 4.33333D;
                                                                        }
                                                                    }
                                                                    if (dateMonth <= 7)
                                                                    {
                                                                        if (pressure > 1020)
                                                                        {
                                                                            return 4.25D;
                                                                        }
                                                                        if (pressure <= 1020)
                                                                        {
                                                                            return 3D;
                                                                        }
                                                                    }
                                                                }
                                                                if (dateDayOfMonth <= 5)
                                                                {
                                                                    return 2.66667D;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (dateMonth <= 4)
                                                {
                                                    if (feelslike == 0)
                                                    {
                                                        return 2.5D;
                                                    }
                                                    if (feelslike > 3.9)
                                                    {
                                                        if (pressure > 1018)
                                                        {
                                                            if (dateDayOfWeek == 0)
                                                            {
                                                                return 2.45455D;
                                                            }
                                                            if (dateDayOfWeek > 1)
                                                            {
                                                                if (feelslike > 27.2)
                                                                {
                                                                    return 1D;
                                                                }
                                                                if (feelslike <= 27.2)
                                                                {
                                                                    if (feelslike > 23.9)
                                                                    {
                                                                        return 3D;
                                                                    }
                                                                    if (feelslike <= 23.9)
                                                                    {
                                                                        if (dateMonth > 3)
                                                                        {
                                                                            return 2D;
                                                                        }
                                                                        if (dateMonth <= 3)
                                                                        {
                                                                            if (feelslike > 12)
                                                                            {
                                                                                return 3D;
                                                                            }
                                                                            if (feelslike <= 12)
                                                                            {
                                                                                return 2D;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (dateDayOfWeek <= 1)
                                                            {
                                                                return 3.5D;
                                                            }
                                                        }
                                                        if (pressure <= 1018)
                                                        {
                                                            return 3.66667D;
                                                        }
                                                    }
                                                    if (feelslike <= 3.9)
                                                    {
                                                        return 1D;
                                                    }
                                                }
                                            }
                                            if (dateMonth <= 2)
                                            {
                                                if (humidity > 90)
                                                {
                                                    return 1D;
                                                }
                                                if (humidity <= 90)
                                                {
                                                    if (feelslike == 0)
                                                    {
                                                        return 4.33333D;
                                                    }
                                                    if (feelslike > 21.6)
                                                    {
                                                        if (pressure > 1091)
                                                        {
                                                            return 3D;
                                                        }
                                                        if (pressure <= 1091)
                                                        {
                                                            return 5D;
                                                        }
                                                    }
                                                    if (feelslike <= 21.6)
                                                    {
                                                        if (humidity > 78)
                                                        {
                                                            return 3D;
                                                        }
                                                        if (humidity <= 78)
                                                        {
                                                            return 4D;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (humidity <= 24)
                                        {
                                            if (dateMonth == 0)
                                            {
                                                return 2.33333D;
                                            }
                                            if (dateMonth > 6)
                                            {
                                                if (dateDayOfWeek == 0)
                                                {
                                                    return 3.2D;
                                                }
                                                if (dateDayOfWeek > 3)
                                                {
                                                    return 4.33333D;
                                                }
                                                if (dateDayOfWeek <= 3)
                                                {
                                                    return 1.5D;
                                                }
                                            }
                                            if (dateMonth <= 6)
                                            {
                                                return 1.25D;
                                            }
                                        }
                                    }
                                }
                                if (pressure <= 1001)
                                {
                                    if (feelslike == 0)
                                    {
                                        return 2.54545D;
                                    }
                                    if (feelslike > 28.3)
                                    {
                                        return 4.5D;
                                    }
                                    if (feelslike <= 28.3)
                                    {
                                        if (dateDayOfWeek == 0)
                                        {
                                            return 2.35D;
                                        }
                                        if (dateDayOfWeek > 4)
                                        {
                                            if (dateDayOfWeek > 6)
                                            {
                                                if (username.Equals("e"))
                                                {
                                                    return 3D;
                                                }
                                                if (!username.Equals("e"))
                                                {
                                                    return 2D;
                                                }
                                            }
                                            if (dateDayOfWeek <= 6)
                                            {
                                                if (dateMonth == 0)
                                                {
                                                    return 1.42857D;
                                                }
                                                if (dateMonth > 9)
                                                {
                                                    return 1.75D;
                                                }
                                                if (dateMonth <= 9)
                                                {
                                                    return 1D;
                                                }
                                            }
                                        }
                                        if (dateDayOfWeek <= 4)
                                        {
                                            if (feelslike > 25.4)
                                            {
                                                return 1.5D;
                                            }
                                            if (feelslike <= 25.4)
                                            {
                                                if (pressure > 992)
                                                {
                                                    return 2.66667D;
                                                }
                                                if (pressure <= 992)
                                                {
                                                    if (feelslike > 12.7)
                                                    {
                                                        return 5D;
                                                    }
                                                    if (feelslike <= 12.7)
                                                    {
                                                        return 3D;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (pressure <= 982)
                            {
                                if (pressure > 948)
                                {
                                    if (username.Equals("e"))
                                    {
                                        if (humidity == 0)
                                        {
                                            return 4.38095D;
                                        }
                                        if (humidity > 95)
                                        {
                                            return 3.5D;
                                        }
                                        if (humidity <= 95)
                                        {
                                            if (dateMonth == 0)
                                            {
                                                return 4.47368D;
                                            }
                                            if (dateMonth > 8)
                                            {
                                                if (feelslike == 0)
                                                {
                                                    return 4.16667D;
                                                }
                                                if (feelslike > 17.85)
                                                {
                                                    return 3D;
                                                }
                                                if (feelslike <= 17.85)
                                                {
                                                    if (humidity > 64)
                                                    {
                                                        return 4.66667D;
                                                    }
                                                    if (humidity <= 64)
                                                    {
                                                        return 4D;
                                                    }
                                                }
                                            }
                                            if (dateMonth <= 8)
                                            {
                                                if (dateDayOfMonth > 7)
                                                {
                                                    return 5D;
                                                }
                                                if (dateDayOfMonth <= 7)
                                                {
                                                    if (pressure > 970)
                                                    {
                                                        return 4D;
                                                    }
                                                    if (pressure <= 970)
                                                    {
                                                        if (feelslike == 0)
                                                        {
                                                            return 4.66667D;
                                                        }
                                                        if (feelslike > 17.7)
                                                        {
                                                            return 4.33333D;
                                                        }
                                                        if (feelslike <= 17.7)
                                                        {
                                                            return 5D;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (!username.Equals("e"))
                                    {
                                        if (pressure > 952)
                                        {
                                            if (humidity == 0)
                                            {
                                                return 3.42857D;
                                            }
                                            if (humidity > 43)
                                            {
                                                if (dateMonth == 0)
                                                {
                                                    return 2.71429D;
                                                }
                                                if (dateMonth > 9)
                                                {
                                                    return 5D;
                                                }
                                                if (dateMonth <= 9)
                                                {
                                                    if (dateDayOfMonth > 8)
                                                    {
                                                        return 1.66667D;
                                                    }
                                                    if (dateDayOfMonth <= 8)
                                                    {
                                                        return 3D;
                                                    }
                                                }
                                            }
                                            if (humidity <= 43)
                                            {
                                                if (dateDayOfWeek == 0)
                                                {
                                                    return 4.14286D;
                                                }
                                                if (dateDayOfWeek > 3)
                                                {
                                                    return 3.5D;
                                                }
                                                if (dateDayOfWeek <= 3)
                                                {
                                                    return 5D;
                                                }
                                            }
                                        }
                                        if (pressure <= 952)
                                        {
                                            return 5D;
                                        }
                                    }
                                }
                                if (pressure <= 948)
                                {
                                    if (dateDayOfMonth > 10)
                                    {
                                        if (humidity == 0)
                                        {
                                            return 2D;
                                        }
                                        if (humidity > 61)
                                        {
                                            if (pressure > 939)
                                            {
                                                return 2D;
                                            }
                                            if (pressure <= 939)
                                            {
                                                return 1D;
                                            }
                                        }
                                        if (humidity <= 61)
                                        {
                                            return 4D;
                                        }
                                    }
                                    if (dateDayOfMonth <= 10)
                                    {
                                        if (humidity == 0)
                                        {
                                            return 3.72D;
                                        }
                                        if (humidity > 95)
                                        {
                                            return 1D;
                                        }
                                        if (humidity <= 95)
                                        {
                                            if (dateDayOfWeek == 0)
                                            {
                                                return 3.83333D;
                                            }
                                            if (dateDayOfWeek > 3)
                                            {
                                                if (pressure > 929)
                                                {
                                                    if (dateMonth == 0)
                                                    {
                                                        return 3.85714D;
                                                    }
                                                    if (dateMonth > 8)
                                                    {
                                                        return 4.75D;
                                                    }
                                                    if (dateMonth <= 8)
                                                    {
                                                        if (humidity > 23)
                                                        {
                                                            if (dateDayOfWeek > 6)
                                                            {
                                                                return 4.5D;
                                                            }
                                                            if (dateDayOfWeek <= 6)
                                                            {
                                                                if (feelslike == 0)
                                                                {
                                                                    return 3.42857D;
                                                                }
                                                                if (feelslike > 5.3)
                                                                {
                                                                    if (humidity > 28)
                                                                    {
                                                                        return 3D;
                                                                    }
                                                                    if (humidity <= 28)
                                                                    {
                                                                        return 4D;
                                                                    }
                                                                }
                                                                if (feelslike <= 5.3)
                                                                {
                                                                    return 4D;
                                                                }
                                                            }
                                                        }
                                                        if (humidity <= 23)
                                                        {
                                                            return 2D;
                                                        }
                                                    }
                                                }
                                                if (pressure <= 929)
                                                {
                                                    return 2.33333D;
                                                }
                                            }
                                            if (dateDayOfWeek <= 3)
                                            {
                                                if (dateMonth == 0)
                                                {
                                                    return 4.42857D;
                                                }
                                                if (dateMonth > 6)
                                                {
                                                    return 3.66667D;
                                                }
                                                if (dateMonth <= 6)
                                                {
                                                    return 5D;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (pressure <= 924)
                        {
                            if (feelslike == 0)
                            {
                                return 3D;
                            }
                            if (feelslike > 18.75)
                            {
                                if (humidity == 0)
                                {
                                    return 3.61538D;
                                }
                                if (humidity > 74)
                                {
                                    if (humidity > 89)
                                    {
                                        return 4D;
                                    }
                                    if (humidity <= 89)
                                    {
                                        return 5D;
                                    }
                                }
                                if (humidity <= 74)
                                {
                                    if (humidity > 59)
                                    {
                                        if (dateMonth == 0)
                                        {
                                            return 2.25D;
                                        }
                                        if (dateMonth > 6)
                                        {
                                            return 1.5D;
                                        }
                                        if (dateMonth <= 6)
                                        {
                                            return 3D;
                                        }
                                    }
                                    if (humidity <= 59)
                                    {
                                        return 4D;
                                    }
                                }
                            }
                            if (feelslike <= 18.75)
                            {
                                if (dateDayOfWeek == 0)
                                {
                                    return 2.55556D;
                                }
                                if (dateDayOfWeek > 6)
                                {
                                    return 4.5D;
                                }
                                if (dateDayOfWeek <= 6)
                                {
                                    if (dateDayOfMonth > 4)
                                    {
                                        if (pressure > 905)
                                        {
                                            if (pressure > 919)
                                            {
                                                if (pressure > 920)
                                                {
                                                    return 2.75D;
                                                }
                                                if (pressure <= 920)
                                                {
                                                    return 4D;
                                                }
                                            }
                                            if (pressure <= 919)
                                            {
                                                if (feelslike > 5.95)
                                                {
                                                    if (dateDayOfMonth > 6)
                                                    {
                                                        return 1.66667D;
                                                    }
                                                    if (dateDayOfMonth <= 6)
                                                    {
                                                        return 3.5D;
                                                    }
                                                }
                                                if (feelslike <= 5.95)
                                                {
                                                    return 1D;
                                                }
                                            }
                                        }
                                        if (pressure <= 905)
                                        {
                                            return 5D;
                                        }
                                    }
                                    if (dateDayOfMonth <= 4)
                                    {
                                        return 1D;
                                    }
                                }
                            }
                        }
                    }
                    if (dateDayOfMonth <= 3)
                    {
                        if (dateMonth == 0)
                        {
                            return 2.82609D;
                        }
                        if (dateMonth > 5)
                        {
                            if (dateDayOfMonth > 1)
                            {
                                if (dateDayOfWeek == 0)
                                {
                                    return 2.88D;
                                }
                                if (dateDayOfWeek > 2)
                                {
                                    if (dateDayOfMonth > 2)
                                    {
                                        if (feelslike == 0)
                                        {
                                            return 2.88889D;
                                        }
                                        if (feelslike > 8.1)
                                        {
                                            if (humidity == 0)
                                            {
                                                return 2.5D;
                                            }
                                            if (humidity > 72)
                                            {
                                                return 2D;
                                            }
                                            if (humidity <= 72)
                                            {
                                                return 3.5D;
                                            }
                                        }
                                        if (feelslike <= 8.1)
                                        {
                                            return 3.66667D;
                                        }
                                    }
                                    if (dateDayOfMonth <= 2)
                                    {
                                        if (feelslike == 0)
                                        {
                                            return 1.85714D;
                                        }
                                        if (feelslike > 22.5)
                                        {
                                            return 3D;
                                        }
                                        if (feelslike <= 22.5)
                                        {
                                            if (humidity == 0)
                                            {
                                                return 1.66667D;
                                            }
                                            if (humidity > 90)
                                            {
                                                return 1D;
                                            }
                                            if (humidity <= 90)
                                            {
                                                if (pressure == 0)
                                                {
                                                    return 1.8D;
                                                }
                                                if (pressure > 935)
                                                {
                                                    return 2D;
                                                }
                                                if (pressure <= 935)
                                                {
                                                    return 1D;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (dateDayOfWeek <= 2)
                                {
                                    if (humidity == 0)
                                    {
                                        return 3.66667D;
                                    }
                                    if (humidity > 52)
                                    {
                                        if (pressure == 0)
                                        {
                                            return 2.8D;
                                        }
                                        if (pressure > 999)
                                        {
                                            return 4D;
                                        }
                                        if (pressure <= 999)
                                        {
                                            if (pressure > 973)
                                            {
                                                return 2D;
                                            }
                                            if (pressure <= 973)
                                            {
                                                return 3D;
                                            }
                                        }
                                    }
                                    if (humidity <= 52)
                                    {
                                        return 4.75D;
                                    }
                                }
                            }
                            if (dateDayOfMonth <= 1)
                            {
                                if (humidity == 0)
                                {
                                    return 4.08333D;
                                }
                                if (humidity > 94)
                                {
                                    return 1D;
                                }
                                if (humidity <= 94)
                                {
                                    if (dateMonth > 9)
                                    {
                                        if (pressure == 0)
                                        {
                                            return 3.8D;
                                        }
                                        if (pressure > 1019)
                                        {
                                            return 4.33333D;
                                        }
                                        if (pressure <= 1019)
                                        {
                                            return 3D;
                                        }
                                    }
                                    if (dateMonth <= 9)
                                    {
                                        if (pressure == 0)
                                        {
                                            return 4.83333D;
                                        }
                                        if (pressure > 1066)
                                        {
                                            return 4D;
                                        }
                                        if (pressure <= 1066)
                                        {
                                            return 5D;
                                        }
                                    }
                                }
                            }
                        }
                        if (dateMonth <= 5)
                        {
                            if (feelslike == 0)
                            {
                                return 2.3125D;
                            }
                            if (feelslike > 28.05)
                            {
                                return 1D;
                            }
                            if (feelslike <= 28.05)
                            {
                                if (feelslike > 26.5)
                                {
                                    return 5D;
                                }
                                if (feelslike <= 26.5)
                                {
                                    if (dateDayOfMonth > 1)
                                    {
                                        if (feelslike > 18.1)
                                        {
                                            if (pressure == 0)
                                            {
                                                return 2.88889D;
                                            }
                                            if (pressure > 1075)
                                            {
                                                return 4D;
                                            }
                                            if (pressure <= 1075)
                                            {
                                                if (feelslike > 23.2)
                                                {
                                                    return 2.33333D;
                                                }
                                                if (feelslike <= 23.2)
                                                {
                                                    return 3D;
                                                }
                                            }
                                        }
                                        if (feelslike <= 18.1)
                                        {
                                            if (humidity == 0)
                                            {
                                                return 2.15385D;
                                            }
                                            if (humidity > 31)
                                            {
                                                if (dateDayOfWeek == 0)
                                                {
                                                    return 2D;
                                                }
                                                if (dateDayOfWeek > 6)
                                                {
                                                    return 1D;
                                                }
                                                if (dateDayOfWeek <= 6)
                                                {
                                                    if (feelslike > 16)
                                                    {
                                                        return 1D;
                                                    }
                                                    if (feelslike <= 16)
                                                    {
                                                        if (feelslike > 8.55)
                                                        {
                                                            return 3D;
                                                        }
                                                        if (feelslike <= 8.55)
                                                        {
                                                            if (feelslike > 7.55)
                                                            {
                                                                return 1D;
                                                            }
                                                            if (feelslike <= 7.55)
                                                            {
                                                                if (pressure == 0)
                                                                {
                                                                    return 2.16667D;
                                                                }
                                                                if (pressure > 970)
                                                                {
                                                                    return 2.33333D;
                                                                }
                                                                if (pressure <= 970)
                                                                {
                                                                    return 2D;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (humidity <= 31)
                                            {
                                                return 3D;
                                            }
                                        }
                                    }
                                    if (dateDayOfMonth <= 1)
                                    {
                                        if (dateDayOfWeek == 0)
                                        {
                                            return 1.85714D;
                                        }
                                        if (dateDayOfWeek > 6)
                                        {
                                            return 1D;
                                        }
                                        if (dateDayOfWeek <= 6)
                                        {
                                            return 2D;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return 0;
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

        /**************************Graph Analysis***********************/
        public ActionResult getMonthData()
        {
            IEnumerable<UserOrder> orderdata = _context.UserOrder;
            IEnumerable<IcecreamTaste> tastesData = _context.IcecreamTaste;
            int tasetsNumber = tastesData.Count();
            var months = from order in orderdata
                         group order by order.Date.Month;

            int monthNumber = months.Count();

            object[,] array = new object[monthNumber + 1, tasetsNumber + 1];

            var ordersByMonth = orderdata.OrderBy(x => x.Date.Month).GroupBy(x => x.Date.Month);
            var tastes = from t in tastesData
                         select t;//group order by months
            array[0, 0] = "Munth";
            int i = 1, j = 1, m = 1;
            string[] monthNames = new string[] {"January", "February", "March", "April", "May",
                     "June", "July", "August", "September", "October", "November", "December"};

            foreach (string month in monthNames) // writing out
            {
                array[m, 0] = month;
                m++;
            }

            foreach (var taste in tastes)// loop all tastes
            {
                var tasteId = taste.Id;
                var name = taste.Name;
                array[0, j] = name;

                // group orders of spesific taste by months
                var sql = from order in orderdata
                          where order.TasteId == tasteId
                          orderby order.Date.Month
                          group order by order.Date.Month;
                // save the count of orders in matrix
                foreach (var item in sql)
                {
                    array[i, j] = item.Count();
                    i++;
                }
                i = 1;
                j++;
            }

            var json = JsonConvert.SerializeObject(array);

            return Ok(json);
        }

    }
}

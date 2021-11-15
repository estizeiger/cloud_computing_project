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

namespace projectDesign.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IceCreamProjectContext _context;
        private double price;

        private readonly IWebHostEnvironment _env;

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
        }

        // GET: IcecreamTastes
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
            var _url = "https://docs.imagga.com/static/images/docs/sample/japan-605234_1280.jpg";
            var request = new RestRequest(Method.GET);
            request.AddParameter("image_url", _url);
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
                if (item.tag.en == "icecrem" && item.confidence > 85|| item.tag.en == "ice" && item.confidence > 20)
                {
                    return true;
                    //Result.Add(item.confidence.ToString(), item.tag.en);
                }
            }
            return  false;
        }
    
    // GET: IcecreamTastes/Details/5
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

        public IActionResult Analysis()
        {
            return View();
        }


        public IActionResult Index()
        {
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
                    
                    return Ok();
                }
                catch (Exception ex)
                {
                    ViewBag.error = $"Exception was thrown: {ex}";
                }
            }
            
            return BadRequest();
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

    }
}

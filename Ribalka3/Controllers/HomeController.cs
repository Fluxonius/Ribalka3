using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ribalka3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Ribalka3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Privacy()
        {
            //Simulate test user data and login timestamp
            Random rnd = new Random();
            var userId =rnd.Next(1000);
            var currentLoginTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            //Save non identifying data to Firebase
            var currentUserLogin = new LoginData() { TimestampUtc = currentLoginTime };
            var firebaseClient = new FirebaseClient("https://ribalka-890d2.firebaseio.com/");
            var result = await firebaseClient
              .Child("Users/" + userId + "/Logins")
              .PostAsync(currentUserLogin);
            //Retrieve data from Firebase
            var dbLogins = await firebaseClient
              .Child("Users")
              .Child(userId.ToString())
              .Child("Logins")
              .OnceAsync<LoginData>();

            var timestampList = new List<DateTime>();

            //Convert JSON data to original datatype
            foreach (var login in dbLogins)
            {
                timestampList.Add(Convert.ToDateTime(login.Object.TimestampUtc).ToLocalTime());
            }

            //Pass data to the view
            ViewBag.CurrentUser = userId;
            ViewBag.Logins = timestampList.OrderByDescending(x => x);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}

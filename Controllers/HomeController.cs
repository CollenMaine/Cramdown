using CramDown.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Hosting;

namespace CramDown.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            FirestoreHelper helper = new FirestoreHelper();
            FirestoreDb db = helper.GetDatabase();
            CollectionReference collection = db.Collection("Posts");
            QuerySnapshot snapshot = await collection.GetSnapshotAsync();
            List<MealModel> list = new List<MealModel>();
            foreach (var document in snapshot.Documents)
            {

                string s = JsonConvert.SerializeObject(document.ToDictionary());
                list.Add(JsonConvert.DeserializeObject<MealModel>(s));
            }
            list.Reverse();
            if (list == null)
                await Index();
            return View(new ViewData<MealModel>(list));
        }

        public IActionResult Terms()  
        {
            return View();
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



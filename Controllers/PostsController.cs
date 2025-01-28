using CramDown.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Firebase.Storage;
using System.Linq;

namespace CramDown.Controllers
{
    public class PostsController : Controller
    {
        private FirestoreHelper helper;
        private FirestoreDb db;

        public PostsController()
        {
            helper = new FirestoreHelper();
            db = helper.GetDatabase();
        }
        public async Task<IActionResult> Post(string id)
        {
            CollectionReference collection = db.Collection("Posts");
            DocumentReference document = collection.Document(id);
            DocumentSnapshot snapshot = await document.GetSnapshotAsync();
            string s = JsonConvert.SerializeObject(snapshot.ToDictionary());
            return View(JsonConvert.DeserializeObject<MealModel>(s));
        }

        public async Task<IActionResult> CreatePost()
        {
            UserModel user = await GetUser(HttpContext.Session.GetString("UserId"));
            if (string.IsNullOrEmpty(user.name))
                return RedirectToAction("Account", "User");
            else
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadPost(MealModel meal)
        {
            IFormFile file = Request.Form.Files.FirstOrDefault();
            UserModel user = await GetUser(HttpContext.Session.GetString("UserId"));
            string link = await FileUpload(file);
            Location location = JsonConvert.DeserializeObject<Location>(HttpContext.Session.GetString("location"));
            meal.uid = user.uid; meal.uName = user.name; meal.uDp = user.image;
            meal.uEmail = user.email; meal.pImage = link; meal.pCity = location.city;
            meal.pCountry = location.country_name;
            helper = new FirestoreHelper();
            db = helper.GetDatabase();
            CollectionReference reference = db.Collection("Posts");
            string a = JsonConvert.SerializeObject(meal);
            Dictionary<string, string> m = JsonConvert.DeserializeObject<Dictionary<string, string>>(a);
            await reference.AddAsync(m);
            return RedirectToAction("UserInfo", "User", new { id = HttpContext.Session.GetString("UserId") });
            //return RedirectToAction("UserInfo", "Posts", HttpContext.Session.GetString("UserId"));
        }

        private async Task<UserModel> GetUser(string id)
        {
            CollectionReference collection = db.Collection("Users");
            DocumentReference document = collection.Document(id);
            DocumentSnapshot snapshot = await document.GetSnapshotAsync();
            string s = JsonConvert.SerializeObject(snapshot.ToDictionary());
            UserModel user = JsonConvert.DeserializeObject<UserModel>(s);
            return user;
        }

        [HttpPost]
        private async Task<string> FileUpload(IFormFile file)
        {
            var stream = file.OpenReadStream();
            var task = new FirebaseStorage(
                "################.appspot.com",
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(HttpContext.Session.GetString("token")),
                        ThrowOnCancel = true,
                    })
                .Child(file.FileName)
                .PutAsync(stream);
            var downloadUrl = await task;
            return downloadUrl;
        }
    }
}
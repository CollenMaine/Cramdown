using CramDown.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Firebase.Auth;
using Firebase.Database;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin;
using Firebase.Storage;
using Google.Cloud.Firestore;
using System.Linq;

namespace CramDown.Controllers
{
    public class UserController : Controller
    {
        private FirebaseConfig authConfig;
        private FirebaseAuthProvider auth;
        private FirebaseClient client;
        private FireSharp.Interfaces.IFirebaseClient firesharpClient;
        private FireSharp.Interfaces.IFirebaseConfig firesharpConfig = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "8888888888888888888888888888888888888",
            BasePath = "*******************************************"
        };
        private string serviceAccountPath = "888888888888888888888888888888888888888888";
        private FirestoreHelper helper;
        private FirestoreDb db;
        private string city = "";

        public UserController()
        {
            helper = new FirestoreHelper();
            db = helper.GetDatabase();
            authConfig = new FirebaseConfig("888888888888888888888888888888888888");
            auth = new FirebaseAuthProvider(authConfig);
            firesharpClient = new FireSharp.FirebaseClient(firesharpConfig);
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserModel user)
        {
            firesharpClient = new FireSharp.FirebaseClient(firesharpConfig);
            try
            {
                var authLink = auth.CreateUserWithEmailAndPasswordAsync(user.email, user.password);
                user.uid = authLink.Result.User.LocalId;
                await firesharpClient.SetAsync("Users/" + user.uid, user);
                await auth.SendEmailVerificationAsync(authLink.Result.FirebaseToken);
            }
            catch
            {
                var user2 = new UserModel();
                user2.isReal = false;
                return View(user2);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public async void SendPasswordLink(UserModel user)
        {
            await auth.SendPasswordResetEmailAsync(user.email);
            RedirectToAction("Login", "User");  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> EditAccount([FromForm]UserModel user)
        {                      
            CollectionReference collection = db.Collection("Users");
            DocumentReference document = collection.Document(HttpContext.Session.GetString("UserId"));
            Dictionary<string, object> dictionary;
            IFormFile profilePicture = Request.Form.Files.FirstOrDefault();
            if (profilePicture != null)
            {
                string link = await Upload(profilePicture);
                dictionary = new Dictionary<string, object>()
                {
                    {"image" , link },
                    {"name", user.name },
                    {"city", user.city }
                };
                DocumentSnapshot snapshot = await document.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    await document.UpdateAsync(dictionary);
                }
            }
            else
            {
                dictionary = new Dictionary<string, object>()
                {
                    {"name", user.name },
                    {"city", user.city }
                };
                DocumentSnapshot snapshot = await document.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    await document.UpdateAsync(dictionary);
                }
            }
            return RedirectToAction("Account");
        }

        public async Task<IActionResult> DeleteAccount()
        {
            var credentials = GoogleCredential.FromFile(serviceAccountPath);
            var firebaseApp = FirebaseApp.Create(new AppOptions
            {
                Credential = credentials
            });
            await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.DeleteUserAsync(HttpContext.Session.GetString("UserId"));
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Account()
        {
            try
            {
                helper = new FirestoreHelper();
                db = helper.GetDatabase();
                CollectionReference collection = db.Collection("Users");
                DocumentReference document = collection.Document(HttpContext.Session.GetString("UserId"));
                DocumentSnapshot snapshot = await document.GetSnapshotAsync();
                string s = JsonConvert.SerializeObject(snapshot.ToDictionary());
                UserModel user = JsonConvert.DeserializeObject<UserModel>(s);
                city = user.city;
                return View(user);
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<string> Upload(IFormFile file)
        {
            var stream = file.OpenReadStream();
            var task = new FirebaseStorage(
                "cramdown-77701.appspot.com",
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

        public async Task<IActionResult> UserInfo(string id)
        {
            FirestoreHelper helper = new FirestoreHelper();
            FirestoreDb db = helper.GetDatabase();
            CollectionReference collection = db.Collection("Users");
            DocumentReference document = collection.Document(id);
            DocumentSnapshot snapshot = await document.GetSnapshotAsync();
            string s = JsonConvert.SerializeObject(snapshot.ToDictionary());
            UserModel user = JsonConvert.DeserializeObject<UserModel>(s);

            collection = db.Collection("Posts");
            Query query = collection.WhereEqualTo("uid", id);
            QuerySnapshot q_snapshot = await query.GetSnapshotAsync();
            List<MealModel> list = new List<MealModel>();
            foreach (var doc in q_snapshot.Documents)
            {
                string temp = JsonConvert.SerializeObject(doc.ToDictionary());
                list.Add(JsonConvert.DeserializeObject<MealModel>(temp));
            }
            list.Reverse();
            UserInfo info = new UserInfo();
            info.meals = list;
            info.user = user;
            return View(info);
        }

        // Everything login begins.
        // Using the Firebase.Auth nuget package for authentication (Easier this way)
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login(UserModel userLogin)
        {
            try
            {
                auth = new FirebaseAuthProvider(authConfig);
                var authLink = await auth.SignInWithEmailAndPasswordAsync(userLogin.email, userLogin.password);
                string token = authLink.FirebaseToken;

                var user = await auth.GetUserAsync(token);
                if (user.IsEmailVerified)
                {
                    HttpContext.Session.SetString("token", token);
                    HttpContext.Session.SetString("UserId", authLink.User.LocalId);
                    HttpContext.Session.SetString("email", userLogin.email);
                    HttpContext.Session.SetString("password", userLogin.password);
                    string ip = LocalIpAddress();
                    Location location = new Location(ip);
                    HttpContext.Session.SetString("location", JsonConvert.SerializeObject(location));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    userLogin.isVerified = false;
                    userLogin.isReal = true;
                    return View(userLogin);
                }
            }
            catch
            {
                var user = new UserModel();
                user.isReal = false;
                return View(user);
            }
        }

        private string LocalIpAddress()
        {
            //string hostName = Dns.GetHostName();
            //return Dns.GetHostAddresses(hostName).Last().ToString();
            //return HttpContext.Connection.LocalIpAddress.ToString();
            return HttpContext.Connection.RemoteIpAddress.ToString();
            //return HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        // Everything login ends.
    }
}


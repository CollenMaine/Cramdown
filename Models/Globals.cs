using Firebase.Auth;
using FireSharp.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Firebase.Database;
using System.Reflection;
using System.Threading.Tasks;

namespace CramDown.Models
{
    public static class Globals
    {
        public static IFirebaseClient client;
        public static IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "#####################################",
            BasePath = "##########################################"
        };
        public static Firebase.Auth.FirebaseConfig authConfig = new Firebase.Auth.FirebaseConfig("#####################################");
        public static IWebHostEnvironment _environment;
        public static string Bucket = "####################################33";
        public static FirebaseAuthProvider auth;
        //public static LocationService locationService;
        public static FirebaseClient firebaseQ = new FirebaseClient("##########################################");

        public static bool IsAnyFieldNullOrEmpty<T>(T myObject)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(myObject);
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                    return true;
            }
            return false;
        }
    }
}

using System.Net;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using MaxMind.GeoIP2;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace CramDown.Models
{
    public class Location
    {
        private string ip { get; set; }
        private string country_code { get; set; }
        public string country_name { get; set; }
        private string region_code { get; set; }
        private string region_name { get; set; }
        public string city { get; set; }
        private string zip { get; set; }
        private double latitude { get; set; }
        private double longitude { get; set; }

        public Location(string _ip)
        {
            GetInfo(_ip);
        }

        public void GetInfo(string _ip)
        {
            string key = "########################################33";
            string Ip_Stack_Url = "http://api.ipstack.com/" + _ip + "?access_key=" + key;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(Ip_Stack_Url);
                HttpResponseMessage response = client.GetAsync(Ip_Stack_Url).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var geolocationInfo = response.Content.ReadAsStringAsync().Result;
                    var info = JsonConvert.DeserializeObject<Location>(geolocationInfo);
                    if (info != null)
                    {
                        country_name = info.country_name;
                        city = info.city;
                    }
                }
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CramDown.Models
{
    public class UserModel : IdentityUser
    {
        public string city { get; set; }
        public string cover { get;set; }
        public string email { get; set; }
        public string image { get; set; }
        //public Location location { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string uid { get; set; }
        [IgnoreDataMember]
        public string password { get; set; }
        [IgnoreDataMember]
        public bool isVerified { get; set; }
        [IgnoreDataMember]
        public bool isReal { get; set; }
    }
}
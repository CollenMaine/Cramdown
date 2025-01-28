using System.Collections.Generic;

namespace CramDown.Models
{
    public class UserInfo
    {
        public List<MealModel> meals = new List<MealModel>();
        public UserModel user { get; set; }   
    }
}

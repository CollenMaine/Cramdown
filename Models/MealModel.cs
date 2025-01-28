using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace CramDown.Models
{
    public class MealModel
    {
        public string pId { get; set; }
        public string Info { get; set; }
        public string pCity { get; set; }
        public string pCategories { get; set; }
        public string pDelivery { get; set; }
        public string pImage { get; set; }
        public int pPrice { get; set; }
        public string pCurrency { get; set; }
        public string pCountry { get; set; }
        public string pTime { get; set; }
        public string pTitle { get; set; }
        public string uDp { get; set; }
        public string uEmail { get; set; }
        public string uName { get; set; }
        public string uid { get; set; }
    }
}
﻿using Microsoft.AspNetCore.Mvc;

namespace CramDown.Controllers
{
    public class LandingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

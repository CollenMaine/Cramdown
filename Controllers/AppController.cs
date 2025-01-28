using Microsoft.AspNetCore.Mvc;

namespace CramDown.Controllers
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

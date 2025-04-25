using Microsoft.AspNetCore.Mvc;

namespace healt_Center.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

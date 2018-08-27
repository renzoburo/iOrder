namespace iOrder.web.Controllers
{
    using Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Client()
        {
            return View();
        }

//        [Route("Home/Product")]
//        public IActionResult Product(string clientId)
//        {
//            return View();
//        }

        public IActionResult Order()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "About Chris Levendall.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact Me.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

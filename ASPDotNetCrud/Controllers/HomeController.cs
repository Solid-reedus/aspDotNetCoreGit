using ASPDotNetCrud.Models;
using ASPDotNetCrud.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;

namespace ASPDotNetCrud.Controlzlers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View("index");
        }


        /*
        public IActionResult uploadFile()
        {
            return View("uploadFile");
        }

        public IActionResult Community()
        {
            return View("community");
        }

        //public IActionResult User()
        //{
        //    return View("user");
        //}

        public IActionResult Login()
        {
            return View("login");
        }

        public IActionResult Register()
        {
            return View("register");
        }

        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
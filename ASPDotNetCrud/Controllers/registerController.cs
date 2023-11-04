using Microsoft.AspNetCore.Mvc;

namespace ASPDotNetCrud.Controllers
{
    public class registerController : Controller
    {
        public IActionResult Register()
        {
            return View("~/Views/Account/register.cshtml");
        }
    }
}

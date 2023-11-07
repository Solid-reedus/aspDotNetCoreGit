using ASPDotNetCrud.Models;
using ASPDotNetCrud.Services;
using ASPDotNetCrud.Utility;
using Microsoft.AspNetCore.Mvc;
using static ASPDotNetCrud.Services.SessionService;

namespace ASPDotNetCrud.Controllers
{
    public class RegisterController : Controller
    {
        private readonly SessionService sessionService;

        public RegisterController(SessionService _sessionService)
        {
            sessionService = _sessionService;
        }

        public IActionResult Register()
        {
            return View("~/Views/Account/register.cshtml");
        }

        // if the name isnt taken make a new user and login as the new user
        public IActionResult MakeUser(string _name, string _password)
        {
            string p = EncryptionUtility.Encrypt(_password);
            bool userExists = MysqlUtility.UserExists(_name, p);

            if (!userExists)
            {
                //set current user as input

                ViewData["loginPopup"] = "";

                //InsertUser
                MysqlUtility.InsertUser(_name, p);

                User currentUser = MysqlUtility.getUser(_name, p);

                sessionService.Set(SessionKeys.userSession, currentUser);
                sessionService.Set(SessionKeys.userName, currentUser.name);

                return RedirectToAction(actionName: "User", controllerName: "userControllers");
            }
            else
            {
                ViewData["loginPopup"] = "username taken";
                return View("~/Views/Account/register.cshtml");
            }

        }

    }
}

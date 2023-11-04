using ASPDotNetCrud.Models;
using ASPDotNetCrud.Utility;
using Microsoft.AspNetCore.Mvc;
using static ASPDotNetCrud.Utility.SessionUtility;

namespace ASPDotNetCrud.Controllers
{
    public class registerController : Controller
    {
        public IActionResult Register()
        {
            return View("~/Views/Account/register.cshtml");
        }


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
                SessionUtility.Set(SessionKeys.userSession, currentUser, HttpContext);
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

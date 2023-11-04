using Microsoft.AspNetCore.Mvc;
using ASPDotNetCrud.Utility;
using static ASPDotNetCrud.Utility.SessionUtility;
using static ASPDotNetCrud.Utility.MysqlUtility;
using ASPDotNetCrud.Models;

namespace ASPDotNetCrud.Controllers
{
    public class LoginController : Controller
    {

        public IActionResult Login()
        {
            return View("~/Views/Account/login.cshtml");
        }


        public IActionResult CheckInput(string _name, string _password)
        {
            string p = EncryptionUtility.Encrypt(_password);
            bool inputIsCorrect = MysqlUtility.UserExists(_name, p);

            if (inputIsCorrect)
            {
                ViewData["loginPopup"] = "";
                User currentUser = MysqlUtility.getUser(_name, p);

                SessionUtility.Set(SessionKeys.userSession, currentUser, HttpContext);
                SessionUtility.Set(SessionKeys.userName, currentUser.name, HttpContext);

                if(!(currentUser.profilePicture == null || currentUser.profilePicture == ""))
                {
                    SessionUtility.Set(SessionKeys.userPic, currentUser.profilePicture, HttpContext);
                }

                return RedirectToAction(actionName: "User", controllerName: "userControllers");
            }
            else
            {
                ViewData["loginPopup"] = "user doesnt exsist";
                return View("~/Views/Account/login.cshtml");
            }

        }
    }
}

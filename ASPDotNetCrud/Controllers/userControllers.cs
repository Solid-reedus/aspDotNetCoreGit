using ASPDotNetCrud.Models;
using ASPDotNetCrud.Utility;
using Microsoft.AspNetCore.Mvc;
using static ASPDotNetCrud.Utility.SessionUtility;

namespace ASPDotNetCrud.Controllers
{
    public class userControllers : Controller
    {
        public IActionResult User()
        {
            User user = SessionUtility.GetUserFromSession(SessionKeys.userSession, HttpContext);
            SessionUtility.Set(SessionKeys.userName, user.name, HttpContext);

            //if (user.profilePicture != null)
            //{
            //    SessionUtility.Set(SessionKeys.userPic, user.profilePicture, HttpContext);
            //}
            ViewData["user"] = user;

            return View("~/Views/user/user.cshtml");
        }
    }
}

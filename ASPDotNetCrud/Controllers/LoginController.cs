﻿using Microsoft.AspNetCore.Mvc;
using ASPDotNetCrud.Utility;
//using static ASPDotNetCrud.Utility.SessionService;
using static ASPDotNetCrud.Services.SessionService;
using static ASPDotNetCrud.Utility.MysqlUtility;
using ASPDotNetCrud.Models;
using ASPDotNetCrud.Services;

namespace ASPDotNetCrud.Controllers
{
    public class LoginController : Controller
    {

        public IActionResult Login()
        {
            return View("~/Views/Account/login.cshtml");
        }

        private readonly SessionService sessionService;

        public LoginController(SessionService _sessionService)
        {
            sessionService = _sessionService;
        }

        // check if the input of the login are correct
        public IActionResult CheckInput(string _name, string _password)
        {
            string p = EncryptionUtility.Encrypt(_password);
            bool inputIsCorrect = MysqlUtility.UserExists(_name, p);

            // if the input is correct set new session for the new current user
            if (inputIsCorrect)
            {
                sessionService.Remove(SessionKeys.userSession);
                sessionService.Remove(SessionKeys.userName);
                sessionService.Remove(SessionKeys.userPic);

                ViewData["loginPopup"] = "";
                User currentUser = MysqlUtility.getUser(_name, p);

                sessionService.Set(SessionKeys.userSession, currentUser);
                sessionService.Set(SessionKeys.userName, currentUser.name);

                if(!(currentUser.profilePicture == null || currentUser.profilePicture == ""))
                {
                    sessionService.Set(SessionKeys.userPic, currentUser.profilePicture);
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

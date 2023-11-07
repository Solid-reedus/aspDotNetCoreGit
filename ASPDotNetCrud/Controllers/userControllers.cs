using ASPDotNetCrud.Models;
using ASPDotNetCrud.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using static ASPDotNetCrud.Services.SessionService;
using static ASPDotNetCrud.Utility.MysqlUtility;
using ASPDotNetCrud.Services;

namespace ASPDotNetCrud.Controllers
{
    public class UserControllers : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SessionService sessionService;

        public UserControllers(IWebHostEnvironment webHostEnvironment, SessionService _sessionService)
        {
            sessionService = _sessionService;
            _webHostEnvironment = webHostEnvironment;
        }

        // get the user session varable and laod the page
        public IActionResult User()
        {
            User user =  sessionService.GetUserFromSession(SessionKeys.userSession);

            if (user == null)
            {
                return View("~/Views/Home/Index.cshtml");
            }
            ViewData["user"] = user;

            return View("~/Views/user/user.cshtml");
        }

        // try to change the name of the user
        // if the username is taken then cancel
        public IActionResult ChangeName(string newName)
        {
            if (MysqlUtility.UserNameIsTaken(newName))
            {
                ViewData["loginPopup"] = "username is taken";
                return View("~/Views/user/user.cshtml");
            }

            // if the new name is set then set the new session
            User? user = sessionService.GetUserFromSession(SessionKeys.userSession);
            MysqlUtility.UpdateUser(user.id, UserProperties.name, newName);
            user.SetNewname(newName);
            sessionService.Set(SessionKeys.userSession, user);
            sessionService.Set(SessionKeys.userName, user.name);
            ViewData["user"] = user;

            return View("~/Views/user/user.cshtml");
        }

        //upload 
        public IActionResult UploadProfilePic(IFormFile imageFile)
        {
            User? user = sessionService.GetUserFromSession(SessionKeys.userSession);
            if (user == null)
            {
                Console.WriteLine("error user is null");
                return View("~/Views/user/user.cshtml");
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                //unique name of the file
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                //route to the uploadfolder 
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadImages");
                // combined path
                var newFilePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the new profile picture
                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                if (System.IO.File.Exists(newFilePath))
                {
                    // Check if the user already had a profile picture
                    if (!string.IsNullOrEmpty(user.profilePicture))
                    {
                        // Delete the old profile picture file
                        string oldFilePath = Path.Combine(uploadsFolder, user.profilePicture);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Update user's profile picture and session variable
                    user.SetProfilePic(uniqueFileName);
                    Console.WriteLine("uniqueFileName = " + uniqueFileName);

                    MysqlUtility.UpdateUser(user.id, UserProperties.profilepic, uniqueFileName);

                    sessionService.Set(SessionKeys.userSession, user);
                    sessionService.Set(SessionKeys.userPic, uniqueFileName);

                    return View("~/Views/user/user.cshtml");
                }
            }
            return View("~/Views/user/user.cshtml");
        }

        // delete the user and clear the session of the user
        public IActionResult DeleteAcount()
        {
            User? user = sessionService.GetUserFromSession(SessionKeys.userSession);
            if (user == null) 
            {
                return View("~/Views/Home/Index.cshtml");
            }

            MysqlUtility.DeleteUser(user.id);

            sessionService.Remove(SessionKeys.userSession);
            sessionService.Remove(SessionKeys.userName);
            sessionService.Remove(SessionKeys.userPic);
            ViewData["user"] = null;

            return View("~/Views/Home/Index.cshtml");
        }


        //clear the session and redirect to the Index
        public IActionResult Logout()
        {
            sessionService.Remove(SessionKeys.userSession);
            sessionService.Remove(SessionKeys.userName);
            sessionService.Remove(SessionKeys.userPic);

            ViewData["user"] = null;

            return View("~/Views/Home/Index.cshtml");
        }

    }
}

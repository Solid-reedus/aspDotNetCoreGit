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

        public IActionResult User()
        {
            Console.WriteLine("user controller \n");

            User user =  sessionService.GetUserFromSession(SessionKeys.userSession);

            //SessionUtility.Set(SessionKeys.userSession, user, HttpContext);

            if (user == null)
            {
                return View("~/Views/Home/Index.cshtml");
            }
            ViewData["user"] = user;

            return View("~/Views/user/user.cshtml");
        }

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
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadImages");
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

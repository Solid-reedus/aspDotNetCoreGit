using ASPDotNetCrud.Models;
using ASPDotNetCrud.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using static ASPDotNetCrud.Utility.SessionUtility;

namespace ASPDotNetCrud.Controllers
{
    public class userControllers : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public userControllers(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult User()
        {
            User user = SessionUtility.GetUserFromSession(SessionKeys.userSession, HttpContext);
            if (user == null)
            {
                return View("~/Views/Home/Index.cshtml");
            }
            ViewData["user"] = user;

            return View("~/Views/user/user.cshtml");
        }

        public IActionResult UploadProfilePic(IFormFile imageFile)
        {


            if (imageFile != null && imageFile.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadImages");
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                //uniqueFileName
            }
            return View("~/Views/user/user.cshtml");
        }

        public IActionResult Logout()
        {
            SessionUtility.Remove(SessionKeys.userSession, HttpContext);
            SessionUtility.Remove(SessionKeys.userName, HttpContext);
            SessionUtility.Remove(SessionKeys.userPic, HttpContext);

            ViewData["user"] = null;

            return View("~/Views/Home/Index.cshtml");
        }

    }
}

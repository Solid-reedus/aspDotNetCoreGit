using ASPDotNetCrud.Models;
using ASPDotNetCrud.Services;
using ASPDotNetCrud.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using static ASPDotNetCrud.Services.SessionService;
using static ASPDotNetCrud.Utility.MysqlUtility;

namespace ASPDotNetCrud.Controllers
{
    public class MakePostController : Controller
    {
        private readonly SessionService sessionService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MakePostController(SessionService _sessionService, IWebHostEnvironment webHostEnvironment)
        {
            sessionService = _sessionService;
            _webHostEnvironment = webHostEnvironment;
        }

        // try to make a new post
        public IActionResult MakePost()
        {
            User? user = sessionService.GetUserFromSession(SessionKeys.userSession);
            uint? communityId = HttpRequestUtility.GETrequest<uint>("pageId", HttpContext);

            // if there is a id for de communityId then add send TempData of the id
            if (communityId != null)
            {
                TempData["communityId"] = communityId.ToString();
            }
            // if there isnt a communityId then return early something went wrong
            else
            {
                ViewData["alert"] = "unable to find id of community";
                return RedirectToAction(actionName: "community", controllerName: "communityController");
            }

            // is there isnt a user return early
            // a post MUST have a user id
            if (user == null) 
            {
                ViewData["alert"] = "unable to find user sesssion";
                return RedirectToAction(actionName: "community", controllerName: "communityController");
            }

            return View("~/Views/Post/makePost.cshtml");
        }

        // make a new post based on the community
        // subtitle and imageFile are optional
        public IActionResult MakeNewPost(string title, string? subtitle, IFormFile imageFile)
        {
            User? user = sessionService.GetUserFromSession(SessionKeys.userSession);
            uint? communityId = null;

            if (uint.TryParse(TempData["communityId"]?.ToString(), out uint val))
            {
                communityId = (uint?)val;
            }

            // if there isnt a a user or communityId return early
            // something went wrong
            if (user == null || communityId == null)
            {
                if (user == null)
                {
                    ViewData["alert"] = "unable to find user sesssion";
                }
                if (communityId == null)
                {
                    ViewData["alert"] = "unable to find community id";
                }
                return RedirectToAction("Community", "community");
            }

            string? uniqueFileName = null;

            if (imageFile != null && imageFile.Length > 0)
            {
                uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadImages");
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
            }

            MysqlUtility.MakeNewPost(title, subtitle, uniqueFileName, user.id, (uint)communityId);

            return RedirectToAction("Community", "community");
        }

    }
}

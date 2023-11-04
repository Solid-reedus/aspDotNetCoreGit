using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using MySqlConnector;
using ASPDotNetCrud.Models;

namespace ASPDotNetCrud.Controllers
{
    public class uploadFileController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public uploadFileController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult uploadFile()
        {
            return View("~/Views/Post/uploadFile.cshtml");
        }

        [HttpPost]
        public IActionResult Upload(IFormFile imageFile)
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
            }

            return View("~/Views/Home/uploadFile.cshtml");
        }
    }
}

using ASPDotNetCrud.Models;
using ASPDotNetCrud.Services;
using ASPDotNetCrud.Utility;
using Microsoft.AspNetCore.Mvc;
using static ASPDotNetCrud.Utility.MysqlUtility;
using static ASPDotNetCrud.Services.SessionService;

namespace ASPDotNetCrud.Controllers
{
    public class NewCommunityController : Controller
    {

        private readonly SessionService sessionService;

        public NewCommunityController(SessionService _sessionService)
        {
            sessionService = _sessionService;
        }

        public IActionResult NewCommunity()
        {
            return View("~/Views/Post/newCommunity.cshtml");
        }
        
        //make a new community
        public IActionResult MakeNewCommunity(string title, string description)
        {
            User? user = sessionService.GetUserFromSession(SessionKeys.userSession);
            if (user != null)
            {
                MysqlUtility.MakeCommunity(title, description, user.id);
            }
            return RedirectToAction("Community", "community");
        }
    }
}

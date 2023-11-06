using ASPDotNetCrud.Models;
using ASPDotNetCrud.Services;
using ASPDotNetCrud.Utility;
using Microsoft.AspNetCore.Mvc;
using static ASPDotNetCrud.Services.SessionService;

namespace ASPDotNetCrud.Controllers
{
    public class communityController : Controller
    {
        private readonly SessionService sessionService;

        public communityController(SessionService _sessionService)
        {
            sessionService = _sessionService;
        }

        public List<Community> GetCommunities()
        {
            return MysqlUtility.GetCommunities();
        }

        public IActionResult Community()
        {
            List<Community> communities = GetCommunities();
            ViewData["communities"] = communities;

            uint? communityId = HttpRequestUtility.GETrequest<uint>("pageId", HttpContext);

            if (communityId != null)
            {
                List<Post>? posts = MysqlUtility.GetCommunityPosts(communityId.Value);

                User? user = sessionService.GetUserFromSession(SessionKeys.userSession);
                if (user != null)
                {
                    ViewData["userId"] = user.id;
                }
                ViewData["posts"] = posts;
                ViewData["communityId"] = communityId;
            }

            return View("~/Views/Post/community.cshtml");
        }
    }
}

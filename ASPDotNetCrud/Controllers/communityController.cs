using ASPDotNetCrud.Models;
using ASPDotNetCrud.Services;
using ASPDotNetCrud.Utility;
using Microsoft.AspNetCore.Mvc;
using static ASPDotNetCrud.Services.SessionService;

namespace ASPDotNetCrud.Controllers
{
    public class CommunityController : Controller
    {
        private readonly SessionService sessionService;

        public CommunityController(SessionService _sessionService)
        {
            sessionService = _sessionService;
        }

        public List<Community> GetCommunities()
        {
            return MysqlUtility.GetCommunities();
        }


        // load Community view and check for data
        // if user is logged in give the view the user
        // if a community is selected "aka get pageId isnt null" then load the post of that Community
        [HttpGet]
        [Route("community")] // <-- this property makes this method able to be used outside this controller
        public IActionResult Community()
        {
            List<Community> communities = GetCommunities();
            ViewData["communities"] = communities;

            uint? communityId = HttpRequestUtility.GETrequest<uint>("pageId", HttpContext);

            if (communityId != null)
            {
                List<Post>? posts = MysqlUtility.GetCommunityPosts(communityId.Value);

                ViewData["posts"] = posts;
                ViewData["communityId"] = communityId;
            }

            User? user = sessionService.GetUserFromSession(SessionKeys.userSession);
            if (user != null)
            {
                ViewData["userId"] = user.id;
            }

            return View("~/Views/Post/community.cshtml");
        }

        //delete the post
        public IActionResult DeletePost(uint posId)
        {
            MysqlUtility.DeletePost(posId);

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

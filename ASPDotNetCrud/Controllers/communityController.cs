using ASPDotNetCrud.Models;
using ASPDotNetCrud.Utility;
using Microsoft.AspNetCore.Mvc;

namespace ASPDotNetCrud.Controllers
{
    public class communityController : Controller
    {


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
                ViewData["posts"] = posts;
                ViewData["communityId"] = communityId;
            }

            return View("~/Views/Post/community.cshtml");
        }
    }
}

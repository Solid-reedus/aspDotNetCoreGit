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

        public List<Post>? GetPosts(uint communityId)
        {
            return null;
        }

        public IActionResult Community()
        {
            List<Community> communities = GetCommunities();
            ViewData["communities"] = communities;

            uint? communityId = HttpRequestUtility.GETrequest<uint>("id", HttpContext);

            if (communityId != null)
            {
                List<Post>? posts = GetPosts(communityId.Value);
                ViewData["communityId"] = communityId;
            }


            return View("~/Views/Post/community.cshtml");
        }

        //SELECT * FROM posts WHERE post_community = 1 LIMIT 0, 10;
        //ORDER BY `questions`.`question_id` DESC
    }
}

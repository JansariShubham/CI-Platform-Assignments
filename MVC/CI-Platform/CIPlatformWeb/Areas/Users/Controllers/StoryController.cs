using Microsoft.AspNetCore.Mvc;

namespace CIPlatformWeb.Areas.Users.Controllers
{
    [Area("Users")]
    public class StoryController : Controller
    {
        public IActionResult ShareStory()
        {
            return View();
        }

        public IActionResult StoryListing()
        {
            return View();
        }
    }

}

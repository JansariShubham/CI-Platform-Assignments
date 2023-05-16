using Microsoft.AspNetCore.Mvc;

namespace CIPlatformWeb.Areas.Users.Controllers
{
    [Area("Users")]
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetNotification()
        {
            return PartialView("_Notification");
        }

        public IActionResult GetNotificationSetting()
        {
            return PartialView("_NotificationSettings");
        }
    }
}

using CIPlatform.entities.DataModels;
using CIPlatform.entities.ViewModels;
using CIPlatform.repository.IRepository;
using CIPlatform.utilities;
using Microsoft.AspNetCore.Mvc;

namespace CIPlatformWeb.Areas.Users.Controllers
{
    [Area("Users")]
    public class StoryController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _IUnitOfWork;
        private readonly EmailSender _EmailSender;


        public StoryController(IUnitOfWork IUnitOfWork, EmailSender emailSender)
        {
            _IUnitOfWork = IUnitOfWork;
            _EmailSender = emailSender;
        }

        public IActionResult ShareStory(string? id)
        {
            var missionList = _IUnitOfWork.MissionApplicationRepository.getAllMissionApplication();
            var missions = missionList.Where(ma => ma.UserId == long.Parse(id!) && ma.ApprovalStatus == 1).Select(ma => ma.Mission);
            List<PlatformLandingViewModel> missionVm = new();
            foreach (var mission in missions)
            {
                missionVm.Add(ConvertToMissionVm(mission));
            }



            ViewBag.missions = missionVm;
            return View();
        }

        private PlatformLandingViewModel ConvertToMissionVm(Mission? mission)
        {
            PlatformLandingViewModel missionVm = new()
            {
                Title = mission.Title,
                MissionId = mission.MissionId
                
            };
            return missionVm;

        }

        public IActionResult StoryListing()
        {
            return View();
        }
    }

}

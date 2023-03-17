using CIPlatform.entities.DataModels;
using CIPlatform.entities.ViewModels;
using CIPlatform.repository.IRepository;
using CIPlatform.utilities;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace CIPlatformWeb.Areas.Users.Controllers
{
    [Area("Users")]
    public class MissionDetailController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _IUnitOfWork;

        private readonly EmailSender _EmailSender;

        public MissionDetailController(IUnitOfWork IUnitOfWork, EmailSender emailSender)
        {
            // userId = this.HttpContext.Session.GetString("userId");
            _IUnitOfWork = IUnitOfWork;
            _EmailSender = emailSender;
        }
        public IActionResult Index(int? id)
        {

            if (id != 0)
            {
                var missionObj = _IUnitOfWork.MissionRepository.GetFirstOrDefault(m => m.MissionId == id);
                var missionVmObj = CovertToMissionVM(missionObj);



                return View(missionVmObj);
            }
            return View();

        }

        public PlatformLandingViewModel CovertToMissionVM(Mission item)
        {
            PlatformLandingViewModel MissionVM = new();
            MissionVM.City = item.City;
            MissionVM.Country = item.Country;
            MissionVM.CountryId = item.CountryId;
            MissionVM.MissionId = item.MissionId;
            MissionVM.CityId = item.CityId;
            MissionVM.Status = item.Status;
            MissionVM.StartDate = item.StartDate;
            MissionVM.EndDate = item.EndDate;
            MissionVM.MissionType = item.MissionType;
            MissionVM.OrgName = item.OrgName;
            MissionVM.ShortDesc = item.ShortDesc;
            MissionVM.Theme = item.Theme;
            MissionVM.ThemeId = item.ThemeId;
            MissionVM.Title = item.Title;
            MissionVM.ThumbnailURL = getUrl(item.MissionMedia);
            return MissionVM;


        }
        public String? getUrl(ICollection<MissionMedia> missionMedia)
        {
            if (missionMedia == null || missionMedia.Count() == 0)
            {
                return null;
            }
            var missionObj = missionMedia.FirstOrDefault(missionMedia => missionMedia.DefaultMedia == true);
            var mediaName = missionObj.MediaName;
            var mediaType = missionObj.MediaType;
            var mediaPath = missionObj.MediaPath;

            var url = mediaPath + mediaName + mediaType;
            return url;
        }

    }
}

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

        //HomeController hcObj = new HomeController();

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
                var missionObj = _IUnitOfWork.MissionRepository.getAllMissions().FirstOrDefault(m => m.MissionId == id);
                
               var missionVm =  HomeController.CovertToMissionVM(missionObj);



                return View(missionVm);
            }
            return View();

        }

        public void AddToFavourite(int? userId, int? missionid)
        {
            if (userId != 0 && missionid != 0)
            {
                FavouriteMission obj = new()
                {
                    MissionId = missionid,
                    UserId = userId

                };

                _IUnitOfWork.FavMissionRepository.Add(obj);
                _IUnitOfWork.Save();


            }

        }

        public void RemoveFromFavourite(int ?userId, int? missionid)
        {
            if (userId != 0 && missionid != 0)
            {
                FavouriteMission obj = new()
                {
                    MissionId = missionid,
                    UserId = userId,
                    

                };

               var favMissionObj =  _IUnitOfWork.FavMissionRepository.GetFirstOrDefault(m => m.UserId == userId && m.MissionId == missionid);

                _IUnitOfWork.FavMissionRepository.Delete(favMissionObj);
                _IUnitOfWork.Save();
            }
        }

        public void AddRatings(int userId, int missionId, byte rating )
        {
            MissionRating missionRatingobj = new()
            {
                UserId = userId,
                MissionId = missionId,
                Rating = rating,
                CreatedAt = DateTimeOffset.Now
            };
            _IUnitOfWork.MissionRatingRepository.Add(missionRatingobj);
            _IUnitOfWork.Save();
        }

    }
}

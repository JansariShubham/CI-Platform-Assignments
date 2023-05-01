using CIPlatform.entities.DataModels;
using CIPlatform.entities.ViewModels;
using CIPlatform.repository.IRepository;
using CIPlatform.repository.Repository;
using CIPlatform.utilities;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Policy;

namespace CIPlatformWeb.Areas.Users.Controllers
{
    [Area("Users")]
    [AuthenticateAdmin]
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

                var missionVm = HomeController.CovertToMissionVM(missionObj);

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

        public void RemoveFromFavourite(int? userId, int? missionid)
        {
            if (userId != 0 && missionid != 0)
            {
                FavouriteMission obj = new()
                {
                    MissionId = missionid,
                    UserId = userId,


                };

                var favMissionObj = _IUnitOfWork.FavMissionRepository.GetFirstOrDefault(m => m.UserId == userId && m.MissionId == missionid);

                _IUnitOfWork.FavMissionRepository.Delete(favMissionObj);
                _IUnitOfWork.Save();
            }
        }

        public IActionResult AddRatings(int userId, int missionId, byte rating)
        {

            _IUnitOfWork.MissionRatingRepository.AddOrUpdateRatings(userId, missionId, rating);

            var missionRatingsObj = _IUnitOfWork.MissionRatingRepository.GetAll();
            var ratingsCnt = missionRatingsObj.Where(mr => mr.MissionId == missionId).Count();

            var ratings = missionRatingsObj.Where(mr => mr.MissionId == missionId).Select(r => convertToMissionRatingVm(r, ratingsCnt));

            var avgRatings = ratings.Average(r => r.Rating);

            ViewBag.volunteerCnt = ratingsCnt;
            ViewBag.avgRatings = avgRatings;

            return PartialView("_Ratings");


            /*
                        _IUnitOfWork.MissionRatingRepository.Add(missionRatingobj);
                        _IUnitOfWork.Save();*/

            /*
                        var ratings = _IUnitOfWork.MissionRatingRepository.GetAll();
                        var countOfRatings = ratings.Count();

                        ViewBag.ratingsByVolunteersCnt = countOfRatings;*/
        }

        [HttpGet]
        public IActionResult getRatings(int? missionId)
        {

            
            var missionRatingsObj = _IUnitOfWork.MissionRatingRepository.GetAll();
            if (missionRatingsObj.Any(m => m.MissionId == missionId))
            {
                var ratingsCnt = missionRatingsObj.Where(mr => mr.MissionId == missionId).Count();

                var ratings = missionRatingsObj.Where(mr => mr.MissionId == missionId).Select(r => convertToMissionRatingVm(r, ratingsCnt));

                var avgRatings = ratings.Average(r => r.Rating);

                ViewBag.volunteerCnt = ratingsCnt;
                ViewBag.avgRatings = avgRatings;
                return PartialView("_Ratings");
            }
            return null;


        }

        public MissionRatingViewModel convertToMissionRatingVm(MissionRating r, int ratingsCnt)
        {
            MissionRatingViewModel ratingVm = new()
            {
                Rating = r.Rating,
                RatingCount = ratingsCnt
            };
            return ratingVm;
        }

        public IActionResult getRelatedMission(int? missionId)
        {
            var missionObj = _IUnitOfWork.MissionRepository.GetFirstOrDefault(m => m.MissionId == missionId);
            if (missionObj != null)
            {


                var missionThemes = _IUnitOfWork.MissionRepository.getAllMissions().Where(m => missionObj.Theme.Title == m.Theme.Title && m.MissionId != missionObj.MissionId);
                List<PlatformLandingViewModel> missionThemeVm = new();

                foreach (var item in missionThemes)
                {
                    missionThemeVm.Add(HomeController.CovertToMissionVM(item));
                }

                // var missionVmThemes = HomeController.CovertToMissionVM(missionThemeVm);
                return PartialView("_Index", new IndexViewModel() { MissionList = missionThemeVm.Take(3).ToList() });

            }
            return null;

        }

        public IActionResult AddComments(int? userId, int? missionId, string? commentText)
        {

            if (userId != 0 && missionId != 0 && commentText != "" && commentText != null)
            {
                Comment commentObj = new()
                {
                    UserId = userId,
                    MissionId = missionId,
                    CommentMsg = commentText,
                    CreatedAt = DateTimeOffset.Now

                };

                _IUnitOfWork.CommentRepository.Add(commentObj);
                _IUnitOfWork.Save();

                var commentList = _IUnitOfWork.CommentRepository.getAllComments();
                var commentsListAsPerMission = commentList.Where(c => c.MissionId == missionId).Select(c => ConvertToCommentVm(c)).ToList();
                return PartialView("_Comments", commentsListAsPerMission);
            }
            return null;




        }
        [HttpGet]
        public IActionResult getAllComments(int? missionId)
        {
            var commentList = _IUnitOfWork.CommentRepository.getAllComments();
            var commentsListAsPerMission = commentList.Where(c => c.MissionId == missionId).Select(c => ConvertToCommentVm(c)).ToList();
            // List<CommentViewModel> commentsVm = new(); 

            return PartialView("_Comments", commentsListAsPerMission);

        }

        public CommentViewModel ConvertToCommentVm(Comment item)
        {
            CommentViewModel commentViewModel = new()
            {
                UserId = item.UserId,
                MissionId = item.MissionId,
                CommentMsg = item.CommentMsg,
                User = item.User,
                CommentId = item.CommentId,
                CreatedAt = item.CreatedAt,
                Mission = item.Mission
            };
            return commentViewModel;

        }
        [HttpGet]
        public IActionResult GetAllUsers(int? userId, int? missionId, int? storyId)
        {
            var usersList = _IUnitOfWork.UserRepository.getAllUsers();
            List<UserViewModel> userVm = new();

            foreach (var user in usersList)
            {
                userVm.Add(convertToUserVm(user));  

            }

            ViewBag.userId = userId;
            ViewBag.missionId = missionId;
            ViewBag.storyId = storyId;
            return PartialView("_Recommend", userVm);
        }




        public UserViewModel convertToUserVm(User? user)
        {
            UserViewModel userVm = new()
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                MissionInviteToUsers= user.MissionInviteToUsers,
                MissionInviteFromUsers= user.MissionInviteFromUsers,
                StoryInviteFromUsers= user.StoryInviteFromUsers,
                StoryInviteToUsers  = user.StoryInviteToUsers,
                

            };
            return userVm;

        }

        public void AddOrRemoveFavourite(int? userId, int? missionId)
        {
            if(userId == null || missionId == null)
            { 
                return;
            }
            var fmObj = _IUnitOfWork.FavMissionRepository.GetFirstOrDefault(fm => fm.UserId == userId && missionId == fm.MissionId);
            
            if(fmObj != null)
            {
                _IUnitOfWork.FavMissionRepository.Delete(fmObj);



            }
            else
            {
                FavouriteMission fm = new()
                {
                    UserId = userId,
                    MissionId = missionId
                };

                _IUnitOfWork.FavMissionRepository.Add(fm);

            }
            _IUnitOfWork.Save();

        }

        public void AddUsersToMissionInvite(int[]? usersIdList, int? missionId, int? currentUserId)
        {
            try
            {
                var currentUser = _IUnitOfWork.UserRepository.GetFirstOrDefault(u => u.UserId == currentUserId);
                /*String subject = "Your Friend" + currentUser.FirstName + "is Recommended you this mission!";
                String htmlMessage = "Please Checkout this Mission!";*/
                var url = Url.Action("Index", "MissionDetail", new { id = missionId }, "https");
                string subject = "CI Platform - Mission Recommendation";
                string link = $"<a href='{url}' style='text-decoration:none;display:block;width:max-content;border:1px solid black;border-radius:5rem;padding:0.75rem 1rem;margin:1rem auto;color:black;font-size:1rem;'>Open mission</a>";
                string htmlMessage = $"<p style='text-align:center;font-size:2rem'>Your co-worker {currentUser.FirstName} has recommended a mission to you.</p><p style='text-align:center;font-size:1.5rem'>Click on the link below check mission out</p><hr/>{link}";
                for (int i = 0; i < usersIdList!.Length; i++)
                {


                    MissionInvite missionInviteObj = new()
                    {
                        FromUserId = currentUserId,
                        MissionId = missionId,
                        ToUserId = usersIdList[i],
                        CreatedAt = DateTimeOffset.Now


                    };

                    _IUnitOfWork.MissionInviteRepository.Add(missionInviteObj);
                    _IUnitOfWork.Save();

                    var userObj = _IUnitOfWork.UserRepository.GetFirstOrDefault(u => u.UserId == usersIdList[i]);
                    if (userObj != null)
                    {
                        _EmailSender.SendEmail(userObj.Email, subject, htmlMessage);
                    }

                }
            }
            catch (Exception ex)
            {
                ex.GetBaseException();
            }



        }

        public IActionResult AddMissionApplication(int missionId , long userId)
        {
            MissionApplication missionAppObj = new()
            { 
                UserId = userId,
                MissionId = missionId,
                AppliedAt = DateTimeOffset.Now,
                ApprovalStatus = 2,
                CreatedAt = DateTimeOffset.Now,
            };
            if(missionAppObj != null)
            {
                _IUnitOfWork.MissionApplicationRepository.Add(missionAppObj);
                _IUnitOfWork.Save();
            }
            return Ok();

        }
        public IActionResult CancelApplication(int missionId, long userId)
        {
            MissionApplication ma = _IUnitOfWork.MissionApplicationRepository.GetFirstOrDefault(ma => ma.MissionId == missionId && ma.UserId == userId);
            _IUnitOfWork.MissionApplicationRepository.Delete(ma);
            _IUnitOfWork.Save();
            return Ok();
        }
    }
}





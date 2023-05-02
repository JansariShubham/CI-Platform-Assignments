using Azure.Core;
using CIPlatform.entities.DataModels;
using CIPlatform.entities.ViewModels;
using CIPlatform.repository.IRepository;
using CIPlatform.repository.Repository;
using CIPlatformWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;
using CIPlatform.utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Linq.Expressions;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.Web.CodeGeneration;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.AspNetCore.Authorization;
//using AspNetCore;

namespace CIPlatformWeb.Areas.Users.Controllers
{
    [Area("Users")]
    [AuthenticateAdmin]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _IUnitOfWork;

        private readonly EmailSender _EmailSender;

        private readonly IWebHostEnvironment _webHostEnvironment;

        // string userId;

        public HomeController(IUnitOfWork IUnitOfWork, EmailSender emailSender, IWebHostEnvironment webHostEnvironment)
        {
            // userId = this.HttpContext.Session.GetString("userId");
            _IUnitOfWork = IUnitOfWork;
            _EmailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
        }

        /*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/
        private BannerViewModel ConvertToBannerVm(Banner banner)
        {
            BannerViewModel vm = new()
            {
                BannerId = banner.BannerId,
                Path = banner.BannerImage,
                SortOrder = banner.SortOrder,
                TextDesc = banner.TextDesc,
                TextTitle = banner.TextTitle,
                Status = (bool)banner.Status,
            };
            return vm;
        }
        public IActionResult Login()
        {
            UserLoginViewModel user = new UserLoginViewModel();
            user.banners = _IUnitOfWork.BannerRepository.GetAll().Select(ConvertToBannerVm).ToList();
            return View(user);
        }
        [HttpPost]
        public IActionResult Login(UserLoginViewModel model)
        {

            if (ModelState.IsValid)
            {
            var adminResult = _IUnitOfWork.UserRepository.GetAdminLoginCredentials(model);
                if(adminResult != null)
                {
                    //HttpContext.Session.SetString("userAvatar", adminResult.Avatar!);
                    HttpContext.Session.SetString("isAdmin", "true");

                    HttpContext.Session.SetString("email", adminResult.Email.ToString());
                    HttpContext.Session.SetString("firstName", adminResult.FirstName!.ToString());
                    HttpContext.Session.SetString("lastName", adminResult.LastName!.ToString());
                    HttpContext.Session.SetString("userId", adminResult.AdminId.ToString());
                    return RedirectToAction("Index", "Dashboard", new {area = "Admin"});   
                }
                var result = _IUnitOfWork.UserRepository.GetLoginCredentials(model);
                if (result != null)
                {
                    if(result?.Status == 1)
                    {

                        TempData["statuserror"] = "You are inactive for some reason!, Please contact admin for login";
                        model.banners = _IUnitOfWork.BannerRepository.GetAll().Select(ConvertToBannerVm).ToList();
                        return View(model);

                    }
                    
                    HttpContext.Session.SetString("email", model.Email.ToString());
                    HttpContext.Session.SetString("firstName", result.FirstName.ToString());
                    HttpContext.Session.SetString("lastName", result.LastName.ToString());
                    HttpContext.Session.SetString("userId", result.UserId.ToString());
                    HttpContext.Session.SetString("avatar", result.Avatar!);


                    

                    return RedirectToAction("PlatFormLandingPage");
                }

                else
                {
                    ModelState.AddModelError("Email", "Please Enter Valid Email");
                    ModelState.AddModelError("Password", "Please Enter Valid Password");
                    model.banners = _IUnitOfWork.BannerRepository.GetAll().Select(ConvertToBannerVm).ToList();
                    return View(model);
                }
            }
            model.banners = _IUnitOfWork.BannerRepository.GetAll().Select(ConvertToBannerVm).ToList();
            return View(model);


        }


        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            ForgotPasswordViewModel f = new ForgotPasswordViewModel();
            f.banners = _IUnitOfWork.BannerRepository.GetAll().Select(ConvertToBannerVm).ToList();
            return View(f);
        }
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            var Email = _IUnitOfWork.UserRepository.GetFirstOrDefault(m => m.Email == model.Email);
            if (ModelState.IsValid)
            {
                if (Email != null)
                {
                    String token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    String email = model.Email;
                    var linkHref = Url.Action("ResetPassword", "Home", new { _email = email, _token = token }, "https");
                    String subject = "Reset Password Link";
                    String htmlMessage = $@"
                            <h2>Welcome Back,</h2>
                            Click below button to reset account's password!<br>
                            <a href='{linkHref}'><button>Reset Your Password</button></a>  
                          ";


                    _EmailSender.SendEmail(email, subject, htmlMessage);

                    PasswordReset obj = new PasswordReset();
                    obj.Email = email;
                    obj.Token = token;
                    _IUnitOfWork.PasswordResetRepo.Add(obj);
                    _IUnitOfWork.Save();

                    TempData["MailSuccess"] = "Reset Password link is Sent to your mail id please check";
                }
                else
                {
                    ModelState.AddModelError("Email", "Please Enter Registerd Email");

                }
            }
            model.banners = _IUnitOfWork.BannerRepository.GetAll().Select(ConvertToBannerVm).ToList();
            return View(model);
        }
        public IActionResult ResetPassword(String _email, String _token)
        {
            ResetPasswordViewModel vm = new ResetPasswordViewModel()
            {
                Email = _email,
                Token = _token
            };

            var ResetPasswordObj = _IUnitOfWork.PasswordResetRepo.GetFirstOrDefault(obj => obj.Token == _token);
            if (ResetPasswordObj == null)
            {
                TempData["ResetPasswordError"] = "Your Reset Password Link Expired!";
                return View("ForgotPassword");

            }
            vm.banners = _IUnitOfWork.BannerRepository.GetAll().Select(ConvertToBannerVm).ToList();
            return View(vm);
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var UserObj = _IUnitOfWork.PasswordResetRepo.GetFirstOrDefault(m => m.Email == model.Email);


            if (UserObj != null)
            {
                var result = _IUnitOfWork.UserRepository.UpadateUserPassword(model.Email, model.Password);
                if (result != 0 && UserObj != null)
                {
                    _IUnitOfWork.PasswordResetRepo.Delete(UserObj);
                    _IUnitOfWork.Save();

                }
                return View("Login");
            }
            model.banners = _IUnitOfWork.BannerRepository.GetAll().Select(ConvertToBannerVm).ToList();
            return View(model);
        }

       
        public IActionResult PlatformLandingPage()
        {
            /*List<Mission> MissionList = obj.ToList();*/
            var indexViewModel = new IndexViewModel()
            {
                CountryList = this.getCountryList(),
                CityList = this.getCityList(),
                MissionList = this.getMissionList(),
                SkillsList = this.getSkillList(),
                ThemeList = this.getThemeList()


            };
            ViewBag.missionCnt = indexViewModel.MissionList.Count();
            indexViewModel.MissionList = indexViewModel.MissionList.Take(4).ToList();
            return View(indexViewModel);
        }

        public List<PlatformLandingViewModel> getMissionList()
        {
            var obj = _IUnitOfWork.MissionRepository.getAllMissions().Where(m => m.IsActive == true).ToList();
            List<PlatformLandingViewModel> MissionVMList = new();


            if (obj != null)
            {
                foreach (var item in obj)
                {
                    MissionVMList.Add(CovertToMissionVM(item));
                }
                return MissionVMList;
            }
            return null;
        }


        public static PlatformLandingViewModel CovertToMissionVM(Mission? item)
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
            MissionVM.OrgDetails = item.OrgDetails;
            MissionVM.RegistrationDeadline = item.RegDeadline;
            MissionVM.Theme = item.Theme;
            MissionVM.ThemeId = item.ThemeId;
            MissionVM.Title = item.Title;
            MissionVM.FavouriteMissionsList = item.FavouriteMissions;
            MissionVM.MissionApplications = item.MissionApplications;
            MissionVM.MissionSkillsList = item.MissionSkills;
            MissionVM.MissionRating = getMissionRatings(item.MissionRatings);
            MissionVM.MissionSkills = getMissionSkillList(item.MissionSkills);
            MissionVM.ThumbnailURL = getUrl(item.MissionMedia);
            MissionVM.MissionRate = item.MissionRatings.Count > 0 ? Math.Ceiling(item.MissionRatings.Average(mr => mr.Rating)) : 0;
            MissionVM.SeatsLeft = item.TotalSeats - item.MissionApplications.Count();
            //MissionVM.MissionDocuments = item.MissionDocuments; 
            // Console.WriteLine("seats lef====>>>>" + MissionVM.SeatsLeft);
            MissionVM.StartDate = item.StartDate;
            MissionVM.EndDate = item.EndDate;
            MissionVM.GoalMissions = getGoalMission(item.GoalMissions);
            MissionVM.MissionDocuments = getMissionDoc(item.MissionDocuments).ToList();


            return MissionVM;
        }



        private static IEnumerable<MissionDocumentVM> getMissionDoc(ICollection<MissionDocument> missionDocuments)
        {
            var missionDoc = missionDocuments.Select(md => new MissionDocumentVM()
            {
                DocumentName = md.DocumentName,
                DocumentPath = md.DocumentPath,
                DocumentType = md.DocumentType,
                DocumentLink = md.DocumentPath + md.DocumentName + md.DocumentType

            });
            return missionDoc;

        }
        private static MissionRating getMissionRatings(ICollection<MissionRating> missionRatings)
        {
            MissionRating rating = new();
            foreach (var missionRating in missionRatings)
            {
                rating.MissionRatingId = missionRating.MissionRatingId;
                rating.UserId = missionRating.UserId;
                rating.MissionId = missionRating.MissionId;


            }

            return rating;


        }

        public static String? getUrl(ICollection<MissionMedia> missionMedia)
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


        public static MissionSkill getMissionSkillList(ICollection<MissionSkill> missionSkills)
        {
            MissionSkill obj = new MissionSkill();
            foreach (var item in missionSkills)
            {


                obj.MissionSkillId = item.MissionSkillId;
                obj.SkillId = item.SkillId;
                obj.Skill = item.Skill;
                obj.MissionId = item.MissionId;
                obj.Mission = item.Mission;
                // obj.Skill.SkillName = item.Skill.SkillName;

            }
            return obj;
        }

        public static GoalMission getGoalMission(ICollection<GoalMission> goalMissions)
        {
            GoalMission obj = new GoalMission();
            foreach (var item in goalMissions)
            {


                obj.GoalMissionId = item.GoalMissionId;
                obj.GoalValue = item.GoalValue;
                obj.GoalObjectiveText = item.GoalObjectiveText;
                obj.CreatedAt = item.CreatedAt;
                obj.GoalAchieved = item.GoalAchieved;


            }
            return obj;
        }


        public List<CityViewModel> getCityList()
        {

            var CityObj = _IUnitOfWork.CityRepository.GetAll().ToList();
            List<CityViewModel> CityVmList = new();

            if (CityObj != null)
            {
                foreach (var city in CityObj)
                {
                    CityVmList.Add(ConvertToCityVm(city));

                }
                return CityVmList;
            }
            return null;
        }


        CityViewModel ConvertToCityVm(City city)
        {
            CityViewModel CityVM = new();
            CityVM.Name = city.Name;
            CityVM.CityId = city.CityId;
            return CityVM;
        }
        public List<CountryViewModel> getCountryList()
        {

            var CountryObj = _IUnitOfWork.CountryRepository.GetAll().ToList();
            List<CountryViewModel> CountryVmList = new();

            if (CountryObj != null)
            {
                foreach (var country in CountryObj)
                {
                    CountryVmList.Add(ConvertToCountryVM(country));
                }
                return CountryVmList;
            }
            return null;
        }

        CountryViewModel ConvertToCountryVM(Country country)
        {
            CountryViewModel CountryVM = new();
            CountryVM.Name = country.Name;
            CountryVM.CountryId = country.CountryId;
            return CountryVM;
        }

        public List<SkillsViewModel> getSkillList()
        {
            var SkillsObj = _IUnitOfWork.SkillsRepository.GetAll().ToList();
            List<SkillsViewModel> SkillsVmList = new();

            if (SkillsObj != null)
            {
                foreach (var Skills in SkillsObj)
                {
                    SkillsVmList.Add(ConvertToSkillsVM(Skills));

                }
                return SkillsVmList;
            }
            return null;
        }


        SkillsViewModel ConvertToSkillsVM(Skill Skills)
        {
            SkillsViewModel SkillsVM = new();
            SkillsVM.SkillName = Skills.SkillName;
            SkillsVM.SkillId = Skills.SkillId;
            return SkillsVM;
        }

        public List<ThemeViewModel> getThemeList()
        {
            var ThemeObj = _IUnitOfWork.MissionThemeRepository.GetAll().ToList();
            List<ThemeViewModel> ThemeViewModelList = new List<ThemeViewModel>();



            if (ThemeObj != null)
            {
                foreach (var Theme in ThemeObj)
                {
                    ThemeViewModelList.Add(ConvertToThemeVM(Theme));
                }
                return ThemeViewModelList;
            }
            return null;

        }


        public ThemeViewModel ConvertToThemeVM(MissionTheme Theme)
        {
            ThemeViewModel ThemeVM = new();
            ThemeVM.Title = Theme.Title;
            ThemeVM.MissionThemeId = Theme.MissionThemeId;
            return ThemeVM;
        }



        public IActionResult Registration()
        {
            UserViewModel user = new();
            user.banners = _IUnitOfWork.BannerRepository.GetAll().Select(ConvertToBannerVm).ToList();
            return View(user);
        }
        [HttpPost]
        public IActionResult Registration(UserViewModel model)
        {
            var userEmail = _IUnitOfWork.UserRepository.GetFirstOrDefault(m => m.Email == model.Email);
            if (userEmail != null)
            {
                ModelState.AddModelError("Email", "Email ID is Already Exist!");
                model.banners = _IUnitOfWork.BannerRepository.GetAll().Select(ConvertToBannerVm).ToList();
                return View(model);
            }

            if (ModelState.IsValid)
            {
                User obj = new User();
                obj.FirstName = model.FirstName;
                obj.LastName = model.LastName;
                obj.Email = model.Email;
                obj.Password = model.Password;
                obj.PhoneNumber = model.PhoneNumber;
                _IUnitOfWork.UserRepository.Add(obj);
                _IUnitOfWork.Save();
                return RedirectToAction("Login");
            }
            
            model.banners = _IUnitOfWork.BannerRepository.GetAll().Select(ConvertToBannerVm).ToList();
            return View(model);
            //return RedirectToAction("ResetPassword");

        }
      
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            var url = Json(Url.Action("PlatformLandingPage", "Home"));
            return url;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        /*[Route("/Users/Home/GetCitiesByCountry")]*/
        public JsonResult GetCitiesByCountry(int country)
        {
            var CountryObj = _IUnitOfWork.CountryRepository.GetFirstOrDefault(countryName => countryName.CountryId == country);
            var cityList = _IUnitOfWork.CityRepository.GetAll().Where(m => m.CountryId == CountryObj.CountryId).ToList();

            return Json(cityList);
        }

        [HttpPost]
        public IActionResult GetFilterData(string? searchText, int[]? cityList, int[]? countryList, int[]? themeList, int[]? skillList, int sortingList, int pageNum, int userId)
        {
            Filters obj = new()
            {
                SearchText = searchText,
                Cties = cityList,
                Countries = countryList,
                Themes = themeList,
                Skills = skillList,
                sortingList = sortingList,
                PageNumber = pageNum,
                userId = userId


            };

            var filterResult = _IUnitOfWork.MissionRepository.getFilters(obj);
            var fr = filterResult.Where(m => m.IsActive == true);
            var fr2 = fr.Select(m => CovertToMissionVM(m));


            //if (pageNum != 0)
            //{
            //    fr.Skip((pageNum - 1) * 4).Take(4).ToList();
            //}
            var indexViewModel = new IndexViewModel()
            {
                CountryList = this.getCountryList(),
                CityList = this.getCityList(),
                MissionList = fr2.ToList(),
                SkillsList = this.getSkillList(),
                ThemeList = this.getThemeList()


            };

            ViewBag.missionCnt = fr.Count();
            indexViewModel.MissionList = indexViewModel.MissionList.Skip((pageNum - 1) * 4).Take(4).ToList();

            return PartialView("_index", indexViewModel);


        }

        public IActionResult UserEditProfile(string? id)
        {
            var userObj = _IUnitOfWork.UserRepository.getAllUsers().FirstOrDefault(u => u.UserId == long.Parse(id!));



            UserProfile userProfileVm = new()
            {
                FirstName = userObj.FirstName,
                LastName = userObj.LastName,
                CityId = userObj.CityId,
               

                CountryId = userObj.CountryId,
                Department = userObj.Departmemt,
                EmployeeId = userObj.EmployeeId,
                LinkedinURL = userObj.LinkedInUrl,
                MyProfile = userObj.ProfileText,
                Title = userObj.Title,
                Avatar = userObj.Avatar,
                Cities = getCityList(),
                Countries = getCountryList(),
                UserSkills = userObj.UserSkills,
                SkillsList = getSkillList(),


                WhyIVolunteer = userObj.WhyIVolunteer,


            };

            return View(userProfileVm);
        }


        [HttpPost]
        public string ChangePassword(string? oldPassword, string? newPassword, string? userId)
        {
            if (ModelState.IsValid)
            {
                if (userId != null)
                {
                    var userPassword = _IUnitOfWork.UserRepository.GetFirstOrDefault(u => u.UserId == long.Parse(userId)).Password;
                    var userEmail = _IUnitOfWork.UserRepository.GetFirstOrDefault(u => u.UserId == long.Parse(userId)).Email;

                    if (userPassword != null)
                    {
                        if (oldPassword!.Equals(userPassword))
                        {
                            _IUnitOfWork.UserRepository.UpadateUserPassword(userEmail, newPassword!);

                        }
                        else
                        {
                            return "error";
                        }
                    }
                }

            }
            return "success";



        }

        public void UpdateUserProfile(IFormFile? profile, int? userId)
        {
            if (profile != null && userId != null)
            {
                deleteFileInFolder(userId);
                //var userObj = _IUnitOfWork.UserRepository.GetFirstOrDefault(u => u.UserId == userId);

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\user_images");
                var extension = Path.GetExtension(profile?.FileName);

                using (var fileStrems = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    profile?.CopyTo(fileStrems);
                }
                string path = @"\images\user_images\";

                var url = path + fileName + extension;

                var isProfileUpdated = _IUnitOfWork.UserRepository.UpdateProfie(userId, url);


                HttpContext.Session.SetString("avatar", url);


            }
        }

        public void deleteFileInFolder(int? userId)
        {

            var userObj = _IUnitOfWork.UserRepository.GetFirstOrDefault(u => u.UserId == userId);

            if (userObj != null)
            {

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                var path = $@"{wwwRootPath}\images\user_images";

                var avatarUrl = userObj.Avatar!.Remove(0, 20);


                var url = Path.Combine(path, avatarUrl);
                System.IO.File.Delete(url!);



            }

        }

        public IActionResult GetContactUsData()
        {

            return PartialView("_ContactUs");
        }

        public void AddContactUsDetails(int userId, string subject, string message)
        {


            ContactUs obj = new()
            {
                UserId = userId,
                Subject = subject,
                Message = message,
                CreatedAt = DateTimeOffset.Now
            };

            _IUnitOfWork.ContactUsRepository.Add(obj);
            _IUnitOfWork.Save();




        }

        public IActionResult PolicyPage()
        {
            return View();
        }

        public IActionResult TimeSheet(int? id)
        {
            var timeSheetList = _IUnitOfWork.TimeSheetRepository.getTimeSheetList().Where(t => t.UserId == id).ToList();

            List<TimeSheetViewModel> timeSheetVmList = new();
            foreach (var item in timeSheetList)
            {
                timeSheetVmList.Add(ConvertToTimeSheetVm(item));
            }
            return View(timeSheetVmList);
        }

        private TimeSheetViewModel ConvertToTimeSheetVm(Timesheet item)
        {
            TimeSheetViewModel vm = new()
            {
                Action = item.Action,
                Date = item.DateVolunteered,
                Hour = item.Hour,
                Minutes = item.Minutes,
                Mission = item.Mission,
                TimeSheetId = item.TimesheetId



            };
            return vm;

        }

        

        private PlatformLandingViewModel ConvertToMissionVm(Mission? mission)
        {
            PlatformLandingViewModel missionVm = new()
            {
                Title = mission!.Title,
                MissionId = mission.MissionId,
                MissionType = mission.MissionType,
                StartDate = mission.StartDate,
                EndDate = mission.EndDate

            };
            return missionVm;

        }

        public IActionResult GetGoalMission(int? userId)
        {
            var missionList = _IUnitOfWork.MissionApplicationRepository.getAllMissionApplication();
            var missions = missionList.Where(ma => ma.UserId == userId && ma.ApprovalStatus == 1 && ma.Mission.MissionType == false).Select(ma => ma.Mission);

            List<PlatformLandingViewModel> missionVm = new();
            foreach (var mission in missions)
            {
                missionVm.Add(ConvertToMissionVm(mission));
            }

            VolunteerGoalViewModel vm = new()
            {
                MissionList = missionVm
            };


            return PartialView("_GoalTimeSheetModal", vm);

        }

        public IActionResult GetTimeMission(int? userId)
        {
            var missionList = _IUnitOfWork.MissionApplicationRepository.getAllMissionApplication();
            var missions = missionList.Where(ma => ma.UserId == userId && ma.ApprovalStatus == 1 && ma.Mission.MissionType == true).Select(ma => ma.Mission);

            List<PlatformLandingViewModel> missionVm = new();
            foreach (var mission in missions)
            {
                missionVm.Add(ConvertToMissionVm(mission));
            }


            VolunteeringHoursViewModel vm = new()
            {
                MissionList = missionVm,
            };


            return PartialView("_HourTimeSheetModal", vm);

        }

        public IActionResult AddHourTimeSheet(int userId, int hours, string message, int minutes, DateTimeOffset date, int missionId)
        {
            if (ModelState.IsValid)
            {
                Timesheet timeSheetObj = new()
                {
                    DateVolunteered = date,
                    CreatedAt = DateTimeOffset.Now,
                    Hour = hours,
                    Notes = message,
                    Minutes = minutes,
                    UserId = userId,
                    MissionId = missionId

                };
                if (timeSheetObj != null)
                {
                    _IUnitOfWork.TimeSheetRepository.Add(timeSheetObj);
                    _IUnitOfWork.Save();
                }

                var timeSheetList = _IUnitOfWork.TimeSheetRepository.getTimeSheetList().Where(t => t.UserId == userId).ToList();

                List<TimeSheetViewModel> timeSheetVmList = new();
                foreach (var item in timeSheetList)
                {
                    timeSheetVmList.Add(ConvertToTimeSheetVm(item));
                }
                return PartialView("_VolTimeSheet", timeSheetVmList);

            }
            return null;
        }


        public IActionResult AddGoalTimeSheet(int userId, string message, int action, DateTimeOffset date, int missionId)
        {
            if (ModelState.IsValid)
            {
                Timesheet timeSheetObj = new()
                {
                    DateVolunteered = date,
                    CreatedAt = DateTimeOffset.Now,

                    Notes = message,
                    Action = action,
                    UserId = userId,
                    MissionId = missionId

                };
                if (timeSheetObj != null)
                {
                    _IUnitOfWork.TimeSheetRepository.Add(timeSheetObj);
                    _IUnitOfWork.Save();
                }


                var timeSheetList = _IUnitOfWork.TimeSheetRepository.getTimeSheetList().Where(t => t.UserId == userId).ToList();

                List<TimeSheetViewModel> timeSheetVmList = new();
                foreach (var item in timeSheetList)
                {
                    timeSheetVmList.Add(ConvertToTimeSheetVm(item));
                }
                return PartialView("_VolTimeSheet", timeSheetVmList);
            }
            return null;

        }

        public IActionResult deleteTimeSheet(long? timeSheetId, int? userId)
        {
            var timeSheetObj = _IUnitOfWork.TimeSheetRepository.GetFirstOrDefault(t => t.TimesheetId == timeSheetId);
            if (timeSheetObj != null)
            {
                _IUnitOfWork.TimeSheetRepository.Delete(timeSheetObj);
                _IUnitOfWork.Save();
            }

            var timeSheetList = _IUnitOfWork.TimeSheetRepository.getTimeSheetList().Where(t => t.UserId == userId).ToList();

            List<TimeSheetViewModel> timeSheetVmList = new();
            foreach (var item in timeSheetList)
            {
                timeSheetVmList.Add(ConvertToTimeSheetVm(item));
            }
            return PartialView("_VolTimeSheet", timeSheetVmList);

        }

        //public IActionResult updateTimeSheet(long? timeSheetId, int? userId)
        //{
        //    var timeSheetObj = _IUnitOfWork.TimeSheetRepository.GetFirstOrDefault(t => t.TimesheetId == timeSheetId);
        //    if (timeSheetObj != null)
        //    {

        //        _IUnitOfWork.Save();
        //    }

        //    var timeSheetList = _IUnitOfWork.TimeSheetRepository.getTimeSheetList().Where(t => t.UserId == userId).ToList();

        //    List<TimeSheetViewModel> timeSheetVmList = new();
        //    foreach (var item in timeSheetList)
        //    {
        //        timeSheetVmList.Add(ConvertToTimeSheetVm(item));
        //    }
        //    return PartialView("_VolTimeSheet", timeSheetVmList);

        //}

        public IActionResult getGoalTimeSheetData(long? timeSheetId, int? userId)
        {
            var timeSheetObj = _IUnitOfWork.TimeSheetRepository.GetFirstOrDefault(t => t.TimesheetId == timeSheetId);

            var missionList = _IUnitOfWork.MissionApplicationRepository.getAllMissionApplication();
            var missions = missionList.Where(ma => ma.UserId == userId && ma.ApprovalStatus == 1 && ma.Mission.MissionType == false).Select(ma => ma.Mission);

            List<PlatformLandingViewModel> missionVm = new();
            foreach (var mission in missions)
            {
                missionVm.Add(ConvertToMissionVm(mission));
            }
            if (timeSheetObj != null)
            {
                VolunteerGoalViewModel vm = new()
                {
                    Action = timeSheetObj.Action,
                    Date = timeSheetObj.DateVolunteered,
                    Message = timeSheetObj.Notes,
                    Mission = timeSheetObj.Mission,
                    MissionList = missionVm
                };

                return PartialView("_EditGoalTimeSheetModal", vm);
            }
            return null;
        }


        public IActionResult getHourTimeSheetData(long? timeSheetId, int? userId)
        {
            var timeSheetObj = _IUnitOfWork.TimeSheetRepository.GetFirstOrDefault(t => t.TimesheetId == timeSheetId);

            var missionList = _IUnitOfWork.MissionApplicationRepository.getAllMissionApplication();
            var missions = missionList.Where(ma => ma.UserId == userId && ma.ApprovalStatus == 1 && ma.Mission.MissionType == true).Select(ma => ma.Mission);

            List<PlatformLandingViewModel> missionVm = new();
            foreach (var mission in missions)
            {
                missionVm.Add(ConvertToMissionVm(mission));
            }
            if (timeSheetObj != null)
            {
                VolunteeringHoursViewModel vm = new()
                {
                    Hour = timeSheetObj.Hour,
                    Minutes = timeSheetObj.Minutes,
                    Date = timeSheetObj.DateVolunteered,
                    Message = timeSheetObj.Notes,
                    Mission = timeSheetObj.Mission,
                    MissionList = missionVm
                };

                return PartialView("_EditHourTimeSheetModal", vm);
            }
            return null;
        }


        public IActionResult UpdateHourTimeSheet(int userId, int hours, string message, int minutes, DateTimeOffset date, int missionId, long timeSheetId)
        {
            if (ModelState.IsValid)
            {
                var timeSheetObj = _IUnitOfWork.TimeSheetRepository.GetFirstOrDefault(t => t.TimesheetId == timeSheetId);

                timeSheetObj.DateVolunteered = date;
                timeSheetObj.CreatedAt = DateTimeOffset.Now;
                timeSheetObj.Hour = hours;
                timeSheetObj.Notes = message;
                timeSheetObj.Minutes = minutes;
                timeSheetObj.UserId = userId;
                timeSheetObj.MissionId = missionId;


                if (timeSheetObj != null)
                {
                    _IUnitOfWork.TimeSheetRepository.Update(timeSheetObj);
                    _IUnitOfWork.Save();
                }

                var timeSheetList = _IUnitOfWork.TimeSheetRepository.getTimeSheetList().Where(t => t.UserId == userId).ToList();

                List<TimeSheetViewModel> timeSheetVmList = new();
                foreach (var item in timeSheetList)
                {
                    timeSheetVmList.Add(ConvertToTimeSheetVm(item));
                }
                return PartialView("_VolTimeSheet", timeSheetVmList);

            }
            return null;
        }


        public IActionResult UpdateGoalTimeSheet(int userId, string message, int action, DateTimeOffset date, int missionId, long timeSheetId)
        {
            if (ModelState.IsValid)
            {
                var timeSheetObj = _IUnitOfWork.TimeSheetRepository.GetFirstOrDefault(t => t.TimesheetId == timeSheetId);
                timeSheetObj.DateVolunteered = date;
                timeSheetObj.CreatedAt = DateTimeOffset.Now;

                timeSheetObj.Notes = message;
                timeSheetObj.Action = action;
                timeSheetObj.UserId = userId;
                timeSheetObj.MissionId = missionId;


                if (timeSheetObj != null)
                {
                    _IUnitOfWork.TimeSheetRepository.Update(timeSheetObj);
                    _IUnitOfWork.Save();
                }


                var timeSheetList = _IUnitOfWork.TimeSheetRepository.getTimeSheetList().Where(t => t.UserId == userId).ToList();

                List<TimeSheetViewModel> timeSheetVmList = new();
                foreach (var item in timeSheetList)
                {
                    timeSheetVmList.Add(ConvertToTimeSheetVm(item));
                }
                return PartialView("_VolTimeSheet", timeSheetVmList);
            }
            return null;

        }



        [HttpPost]
        public IActionResult SaveUserDetail(short? CityId,string? MyProfile, byte? CountryId, string? Department , string? EmployeeId,string? FirstName, string? LastName,string? Title, string? WhyIVolunteer, string? LinkedinURL,byte? Avaibility, int[]? userSkillsId, int? userId)
        {

          var userObj =   _IUnitOfWork.UserRepository.GetFirstOrDefault(u => u.UserId == userId);

            userObj.CityId = CityId;
            userObj.ProfileText = MyProfile;
           userObj.CountryId = CountryId;
            userObj.UpdatedAt = DateTimeOffset.Now;
           userObj.Departmemt = Department;
            userObj.EmployeeId = EmployeeId;
            userObj.FirstName = FirstName;
            userObj.LastName = LastName;
            userObj.Title = Title;
           userObj.WhyIVolunteer = WhyIVolunteer;
            userObj.LinkedInUrl = LinkedinURL;
           userObj.Avaibility = Avaibility;


                
                _IUnitOfWork.UserRepository.Update(userObj);
            _IUnitOfWork.Save();


            var SkillObj = _IUnitOfWork.UserSkillRepository.GetAll().Where(u => u.UserId == userId);
            if (SkillObj != null)
            {
                _IUnitOfWork.UserSkillRepository.RemoveRange(SkillObj);
                _IUnitOfWork.Save();
            }
            for (int i = 0; i < userSkillsId?.Length; i++)
            {
                UserSkill userSkillObj = new()
                {
                    SkillId = userSkillsId[i],
                    CreatedAt = DateTimeOffset.Now,
                    UserId = userId
                };

                _IUnitOfWork.UserSkillRepository.Add(userSkillObj);
                _IUnitOfWork.Save();
            }

            TempData["EditProfileSuccess"] = "Profile updated successfully";
            HttpContext.Session.SetString("firstName", FirstName.ToString());
            HttpContext.Session.SetString("lastName", LastName.ToString());
            
            return  RedirectToAction("PlatFormLandingPage");


        }
        private CmsViewModel ConvertToCmsVm(CmsPage cms)
        {
            CmsViewModel cmsVm = new()
            {
                CmsPageId = cms.CmsPageId,
                Description = cms.Description,
                Slug = cms.Slug,
                Status = cms.Status,
                Title = cms.Title
            };
            return cmsVm;
        }
        public IActionResult CmsPage(long id)
        {
            try
            {
                CmsViewModel cms = ConvertToCmsVm(_IUnitOfWork.CmsRepository.GetFirstOrDefault(c => c.CmsPageId == id));
                return View("Cms", cms);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home", new { area = "Users" });
            }
        }
        public IActionResult GetCmsList()
        {
            List<CmsViewModel> cmsPageVMs = _IUnitOfWork.CmsRepository.GetAll().Select(ConvertToCmsVm).ToList();
            return Json(cmsPageVMs.Select(cms => new { cms.Title, cms.CmsPageId }));
        }
    }
}

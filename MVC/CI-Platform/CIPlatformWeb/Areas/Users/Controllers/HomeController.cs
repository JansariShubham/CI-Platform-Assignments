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

namespace CIPlatformWeb.Areas.Users.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _IUnitOfWork;

        private readonly EmailSender _EmailSender;

        public HomeController(IUnitOfWork IUnitOfWork, EmailSender emailSender)
        {
            _IUnitOfWork = IUnitOfWork;
            _EmailSender = emailSender;
        }

        /*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserLoginViewModel model)
        {

            var result = _IUnitOfWork.UserRepository.GetLoginCredentials(model);


            if (ModelState.IsValid)
            {
                if ( result != null)
                {
                    HttpContext.Session.SetString("email", model.Email.ToString());
                    HttpContext.Session.SetString("firstName", result.FirstName.ToString());

                    return View("PlatFormLandingPage");
                }

                else
                {
                    ModelState.AddModelError("Email", "Please Enter Valid Email");
                    ModelState.AddModelError("Password", "Please Enter Valid Password");
                    return View();
                }
            }
            return View();
            

        }


        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            var Email = _IUnitOfWork.UserRepository.GetFirstOrDefault(m=> m.Email==model.Email);
            if (ModelState.IsValid)
            {
                if (Email != null)
                {
                    String token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    String email = model.Email;
                    var linkHref =  Url.Action("ResetPassword", "Home", new {_email = email, _token = token }, "https");
                    String subject = "Reset Password Link";
                    String htmlMessage = $@"
                            <h2>Welcome Back,</h2>
                            Click below button to reset account's password!<br>
                            <a href='{linkHref}'><button>Reset Your Password</button></a>  
                          ";
                    

                    _EmailSender.SendEmail(email, subject, htmlMessage);

                    PasswordReset obj = new PasswordReset();    
                    obj.Email = email;
                    obj.Token= token;
                    _IUnitOfWork.PasswordResetRepo.Add(obj);
                    _IUnitOfWork.Save();
                    
                    TempData["MailSuccess"] = "Reset Password link is Sent to your mail id please check";
                }
                else
                {
                    ModelState.AddModelError("Email", "Please Enter Registerd Email");
                    
                }
            }
            return View();
        }
        public IActionResult ResetPassword(String _email, String _token)
        {
            ResetPasswordViewModel vm = new ResetPasswordViewModel()
            { 
                Email = _email,
                Token = _token
            };

            var ResetPasswordObj = _IUnitOfWork.PasswordResetRepo.GetFirstOrDefault(obj => obj.Token == _token);
            if(ResetPasswordObj == null)
            {
                TempData["ResetPasswordError"] = "Your Reset Password Link Expired!";
                return View("ForgotPassword");

            }
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

            return View();
        }
        public IActionResult PlatformLandingPage()
        {
            try
            {
                var obj = _IUnitOfWork.MissionRepository.GetAll().ToList();
                if(obj != null)
                { 
                     PlatformLandingViewModel MissionVM = new PlatformLandingViewModel();
                    foreach (var item in obj)
                    {
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
                        
                    }
                }

                var CityObj = _IUnitOfWork.CityRepository.GetAll().ToList();

                CityViewModel CityVM = new();
                if (CityObj != null)
                {
                    foreach(var city in CityObj)
                    {
                        CityVM.Name = city.Name;
                    }
                }

                var CountryObj = _IUnitOfWork.CountryRepository.GetAll().ToList();
                CountryViewModel CountryVM = new();
                if(CountryObj != null)
                {
                    foreach(var country in CountryObj)
                    {
                        CountryVM.Name = country.Name;
                    }
                }

                var SkillsObj = _IUnitOfWork.SkillsRepository.GetAll().ToList();
                SkillsViewModel SkillsVM = new();
                if (SkillsObj != null)
                {
                    foreach (var Skills in SkillsObj )
                    {
                        SkillsVM.SkillName = Skills.SkillName;
                    }
                }

                var ThemeObj = _IUnitOfWork.MissionThemeRepository.GetAll().ToList();
                List<ThemeViewModel> ThemeViewModelList = new List<ThemeViewModel>();
                ThemeViewModel ThemeVM = new();


                if (ThemeObj != null)
                {
                    foreach (var Theme in ThemeObj)
                    {
                        ThemeViewModelList.Add(ConvertToThemeVM(Theme));
                    }
                }
                ThemeViewModel ConvertToThemeVM(MissionTheme Theme)
                {
                    ThemeVM.Title = Theme.Title;
                    return ThemeVM;
                }


                /*List<Mission> MissionList = obj.ToList();*/

            }
            catch(Exception e)
            {
                e.GetBaseException();
            }
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(UserViewModel model)
        {
           var userEmail =_IUnitOfWork.UserRepository.GetFirstOrDefault(m => m.Email == model.Email);
            if(userEmail != null)
            {
                ModelState.AddModelError("Email","Email ID is Already Exist!");
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


            return RedirectToAction("ResetPassword");

        }
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
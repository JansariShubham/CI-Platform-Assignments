using CIPlatform.entities.DataModels;
using CIPlatform.entities.ViewModels;
using CIPlatform.repository.IRepository;
using CIPlatform.repository.Repository;
using CIPlatformWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;

namespace CIPlatformWeb.Areas.Users.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _IUnitOfWork;

        public HomeController(IUnitOfWork IUnitOfWork)
        {
            _IUnitOfWork = IUnitOfWork;
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

            
            var UserMail = _IUnitOfWork.UserRepository.GetFirstOrDefault(m => m.Email == model.Email);
            var UserPassword = _IUnitOfWork.UserRepository.GetFirstOrDefault(m => m.Password == model.Password);
            
            if (ModelState.IsValid)
            {
                if (UserMail != null && UserPassword != null)
                {
                    HttpContext.Session.SetString("email", UserMail.ToString());
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
            if (ModelState.IsValid)
            {


            }
            return View();
        }
        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult PlatformLandingPage()
        {
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
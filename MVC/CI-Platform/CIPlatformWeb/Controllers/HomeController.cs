using CIPlatform.entities.DataModels;
using CIPlatform.entities.ViewModels;
using CIPlatform.repository.IRepository;
using CIPlatformWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CIPlatformWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUserRepository IUserRepo;

        public HomeController(IUserRepository UserRepo)
        {
            this.IUserRepo = UserRepo;
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
            User obj = IUserRepo.GetFirstOrDefault(m => m.Email == model.Email);
            if (ModelState.IsValid)
            {
                if (obj.Password == model.Password && obj.Email == model.Email)
                    return View("Registration");

            }
            return RedirectToAction("ForgotPassword");
            
        }


        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult ResetPassword()
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
            if (ModelState.IsValid)
            {
                Console.WriteLine("Registration...");
                User obj = new User();
                obj.FirstName = model.FirstName;
                obj.LastName = model.LastName;
                obj.Email = model.Email;
                obj.Password = model.Password;
                obj.PhoneNumber = model.PhoneNumber;
                IUserRepo.Add(obj);
                IUserRepo.save(obj);
                return RedirectToAction("Index");
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
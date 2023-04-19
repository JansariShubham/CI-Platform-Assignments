using CIPlatform.entities.DataModels;
using CIPlatform.entities.ViewModels;
using CIPlatform.repository.IRepository;
using CIPlatform.utilities;
using CIPlatformWeb.Areas.Users.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CIPlatformWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _IUnitOfWork;

        private readonly EmailSender _EmailSender;

        public DashboardController(ILogger<HomeController> logger, IUnitOfWork iUnitOfWork, EmailSender emailSender)
        {
            _logger = logger;
            _IUnitOfWork = iUnitOfWork;
            _EmailSender = emailSender;
        }
    
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult getAllUsers()
        {
            var userList = _IUnitOfWork.UserRepository.GetAll().ToList();
            List<UserProfile> userVmList = new();
            foreach(var user in userList)
            {
                userVmList.Add(ConverToUserVm(user));
            }
            return PartialView("_User", userVmList);

        }

        private UserProfile ConverToUserVm(User user)
        {
            UserProfile userVm = new()
            {
                userId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                email = user.Email,
                EmployeeId = user.EmployeeId,
                Department = user.Departmemt,
                Status = user.Status,

            };
            return userVm;
        }

        public IActionResult getAddUserModal()
        {
            return PartialView("_AddUser");
        }

        public IActionResult AddUser(string? firstName , string? lastName , string? email, string? empId, string? department, string? password )
        {
            User userObj = new()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                EmployeeId = empId,
                Departmemt = department,
                Password = password,
                CreatedAt = DateTimeOffset.Now
            };
            if (userObj != null)
            {
                _IUnitOfWork.UserRepository.Add(userObj);
                _IUnitOfWork.Save();
             }

            var userList = _IUnitOfWork.UserRepository.GetAll().ToList();
            List<UserProfile> userVmList = new();
            foreach (var user in userList)
            {
                userVmList.Add(ConverToUserVm(user));
            }
            return PartialView("_User", userVmList);
            
            
        }

        public IActionResult getUserDataByUserId(int userId)
        {
           var userObj =  _IUnitOfWork.UserRepository.GetFirstOrDefault(u => u.UserId == userId);
            UserProfile userVm = new()
            { 
                FirstName = userObj.FirstName,
                LastName = userObj.LastName,
                email = userObj.Email,
                Department = userObj.Departmemt,
                Password = userObj.Password,
                EmployeeId = userObj.EmployeeId,
            };
            return PartialView("_EditUser", userVm);
        }


        public IActionResult editUser(string? firstName, string? lastName, string? email, string? empId, string? department, string? password, long? userId)
        {

            var userObj = _IUnitOfWork.UserRepository.GetFirstOrDefault(u => u.UserId == userId);

            userObj.FirstName = firstName;
            userObj.LastName = lastName;
            userObj.Email = email;
            userObj.EmployeeId = empId;
            userObj.Departmemt = department;
            userObj.Password = password;
            userObj.UpdatedAt = DateTimeOffset.Now;
            
            if (userObj != null)
            {
                _IUnitOfWork.UserRepository.Update(userObj);
                _IUnitOfWork.Save();
            }

            var userList = _IUnitOfWork.UserRepository.GetAll().ToList();
            List<UserProfile> userVmList = new();
            foreach (var user in userList)
            {
                userVmList.Add(ConverToUserVm(user));
            }
            return PartialView("_User", userVmList);


        }

        public IActionResult changeUserStatus(long? userId, byte? status)
        {
             
            if (status == 0)
            {
                var isStatusChange = _IUnitOfWork.UserRepository.ChangeUserStatus(userId, 1);
            }
            else
            {
               var  isStatusChange = _IUnitOfWork.UserRepository.ChangeUserStatus(userId, 0);
            }
            var userList = _IUnitOfWork.UserRepository.GetAll().ToList();
            List<UserProfile> userVmList = new();
            foreach (var user in userList)
            {
                userVmList.Add(ConverToUserVm(user));
            }
            return PartialView("_User", userVmList);
        }

        public IActionResult getSearchedUsers(string? searchText)
        {
           List<User> userList =  _IUnitOfWork.UserRepository.getSearchedResult(searchText);
            List<UserProfile> userVmList = new();
            foreach (var user in userList)
            {
                userVmList.Add(ConverToUserVm(user));
            }
            return PartialView("_User", userVmList);

        }
    }

    
}

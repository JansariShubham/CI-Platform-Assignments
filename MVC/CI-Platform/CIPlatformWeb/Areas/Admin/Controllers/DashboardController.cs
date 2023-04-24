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
            if (userList != null)
            {
                List<UserProfile> userVmList = new();
                foreach (var user in userList)
                {
                    userVmList.Add(ConverToUserVm(user));
                }
                return PartialView("_User", userVmList);
            }
            else
            {
                var usersList = _IUnitOfWork.UserRepository.GetAll().ToList();
                List<UserProfile> userVmList = new();
                foreach (var user in usersList)
                {
                    userVmList.Add(ConverToUserVm(user));
                }

            return PartialView("_User", userVmList);
            }

        }


        public IActionResult getCmsDetails()
        {
            var cmsList = _IUnitOfWork.CmsRepository.GetAll().ToList();
            List<CmsViewModel> cmsVmList = new();
            if (cmsList != null)
            {
                foreach(var cms in cmsList)
                {
                    cmsVmList.Add(ConvertToCmsVm(cms));
                }
            }
            return PartialView("_CMS",cmsVmList);
        }

        private CmsViewModel ConvertToCmsVm(CmsPage cms)
        {
            CmsViewModel cmsVm = new()
            {
                CmsPageId= cms.CmsPageId,
                Description = cms.Description,
                Slug = cms.Slug,
                Status = cms.Status,
                Title = cms.Title
            };
            return cmsVm;
        }

        public IActionResult cmsAddPage()
        {
            return PartialView("_AddCms");
        }

        [HttpPost]
        public IActionResult AddCmsDetails(string? title, string? desc, string? slug , bool status)
        {
            
            CmsPage obj = new()
            {
                    CreatedAt = DateTimeOffset.Now,
                    Description = desc,
                    Slug = slug!,
                    Status = status,
                    Title = title!,

                };
              _IUnitOfWork.CmsRepository.Add(obj);
                _IUnitOfWork.Save();
            return StatusCode(200);
        }


        public IActionResult changeCmsStatus(int cmsId, byte status)
        {
            if(status == 1)
            {
                _IUnitOfWork.CmsRepository.ChangeCmsStatus(cmsId, 0);
            }
            else
            {
                _IUnitOfWork.CmsRepository.ChangeCmsStatus(cmsId, 1);

            }

            return Ok(200);
        }

        public IActionResult getCmsEditDetails(int cmsId)
        {
           var cmsObj = _IUnitOfWork.CmsRepository.GetFirstOrDefault(c => c.CmsPageId == cmsId);
           
                CmsViewModel cmsVm = new()
                {
                    CmsPageId = cmsObj.CmsPageId,
                    Description = cmsObj.Description,
                    Slug = cmsObj.Slug,
                    Status = cmsObj.Status,
                    Title= cmsObj.Title,
                    

                };
                return PartialView("_EditCms", cmsVm);
            
        }

        public IActionResult editCmsDetails(string desc, string title, string slug, bool status, int cmsId)
        {   
            var cmsObj = _IUnitOfWork.CmsRepository.GetFirstOrDefault(c => c.CmsPageId == cmsId);

            cmsObj.Status = status;
            cmsObj.Title = title;
            cmsObj.UpdatedAt = DateTimeOffset.Now;
            cmsObj.Slug = slug;
            cmsObj.Description = desc;

            _IUnitOfWork.CmsRepository.Update(cmsObj);
            _IUnitOfWork.Save();
            return Ok(200);
        }


        public IActionResult getSearchedCms(string? searchText)
        {
                List<CmsPage> cmsSerachedList = _IUnitOfWork.CmsRepository.getSearchedCms(searchText);
                if (cmsSerachedList != null)
                {
                    List<CmsViewModel> cmsVmList = new();

                    foreach (var cms in cmsSerachedList)
                    {
                        cmsVmList.Add(ConvertToCmsVm(cms));
                    }

                    return PartialView("_CMS", cmsVmList);
                }
            
            else
            {
                var cmsList = _IUnitOfWork.CmsRepository.GetAll().ToList();
                List<CmsViewModel> cmsVmList = new();
                if (cmsList != null)
                {
                    foreach (var cms in cmsList)
                    {
                        cmsVmList.Add(ConvertToCmsVm(cms));
                    }
                }
                return PartialView("_CMS", cmsVmList);
            }
          

        }

        public IActionResult getMissionThemeList()
        {
           var missionThemeList =  _IUnitOfWork.MissionThemeRepository.GetAll().ToList();

            List<ThemeViewModel> themeVm = new();
            if (missionThemeList != null)
            {
                foreach(var theme in missionThemeList)
                {
                    themeVm.Add(ConvertToMissionThemeVm(theme));
                }
            }
            return PartialView("_MissionTheme", themeVm);
        }

        private ThemeViewModel ConvertToMissionThemeVm(MissionTheme theme)
        {
            ThemeViewModel themeViewModel = new()
            {
                MissionThemeId = theme.MissionThemeId,
                Status = theme.Status,
                Title = theme.Title,
            };
            return themeViewModel;
        }

        public IActionResult getAddThemeDetails()
        {
            return PartialView("_AddMissionTheme");
        }


        public IActionResult AddMissionTheme(ThemeViewModel model)
        {
            if(model != null)
            {
                MissionTheme obj = new()
                {
                    Status= model.Status,
                    Title = model.Title,
                    CreatedAt = DateTimeOffset.Now
                };

                _IUnitOfWork.MissionThemeRepository.Add(obj);
                _IUnitOfWork.Save();

            }
            return Ok(200);
        }

        public IActionResult ChangeMissionThemeStatus(int missionThemeId,byte missionThemeStatus)
        {
            if(missionThemeStatus == 1)
            {
                _IUnitOfWork.MissionThemeRepository.ChangeThemeStatus(missionThemeId, 0);
            }
            else
            {
                _IUnitOfWork.MissionThemeRepository.ChangeThemeStatus(missionThemeId, 1);
            }

        return Ok(200);
        }


        public IActionResult GetThemeEditForm(int themeId)
        {
          var themeObj = _IUnitOfWork.MissionThemeRepository.GetFirstOrDefault(mt => mt.MissionThemeId == themeId);
            ThemeViewModel themeVm = new()
            { 
                Title = themeObj.Title,
                Status = themeObj.Status,
                
            };
            return PartialView("_EditMissionTheme", themeVm);

        }

        public IActionResult EditThemeData(string title,byte status , int themeId)
        {
            var themeObj = _IUnitOfWork.MissionThemeRepository.GetFirstOrDefault(mt => mt.MissionThemeId == themeId);
            themeObj.Title = title;
            themeObj.Status = status;
            themeObj.UpdatedAt = DateTimeOffset.Now;
            _IUnitOfWork.MissionThemeRepository.Update(themeObj);
            _IUnitOfWork.Save();
            return Ok(200);
        }

        public IActionResult getSearchedThemes(string searchText)
        {
            var searchedThemeList = _IUnitOfWork.MissionThemeRepository.getSearchedThemeList(searchText);
            if(searchedThemeList != null)
            {
                List<ThemeViewModel> themeVm = new();
                
                    foreach (var theme in searchedThemeList)
                    {
                        themeVm.Add(ConvertToMissionThemeVm(theme));
                    }
                return PartialView("_MissionTheme", themeVm);
            }
            else
            {
                var missionThemeList = _IUnitOfWork.MissionThemeRepository.GetAll().ToList();
                List<ThemeViewModel> themeVm = new();
                if (missionThemeList != null)
                {
                    foreach (var theme in missionThemeList)
                    {
                        themeVm.Add(ConvertToMissionThemeVm(theme));
                    }
                }
                return PartialView("_MissionTheme", themeVm);
            }
        }


        public IActionResult GetSkillList()
        {
           var skillList =  _IUnitOfWork.SkillsRepository.GetAll().ToList();
            List<SkillsViewModel> skillVmList = new();

            foreach(var skill in skillList)
            {
                skillVmList.Add(ConvertToSkillVm(skill));
            }
            return PartialView("_MissionSkill", skillVmList);
            
        }

        private SkillsViewModel ConvertToSkillVm(Skill skill)
        {
            SkillsViewModel skillVm = new()
            {
                SkillId = skill.SkillId,
                SkillName = skill.SkillName,
                Status = skill.Status
                
            };
            return skillVm;
        }

        public IActionResult GetAddSkillForm()
        {
            return PartialView("_AddSkill");
        }

        public IActionResult AddSkill(SkillsViewModel model)
        {
            Skill obj = new()
            {
                SkillName= model.SkillName,
            };
            _IUnitOfWork.SkillsRepository.Add(obj);
            _IUnitOfWork.Save();
            return Ok(200);
        }

        [HttpPost]
        public IActionResult ChangeSkillStatus(bool status, int skillId)
        {
            if (status) {
                _IUnitOfWork.SkillsRepository.ChangeSkillsStatus(0, skillId);
             }
            else
            {
                _IUnitOfWork.SkillsRepository.ChangeSkillsStatus(1, skillId);
            }
            return Ok();
        }

        
        public IActionResult GetSkillEditForm(int skillId)
        {
            var skillObj = _IUnitOfWork.SkillsRepository.GetFirstOrDefault(s => s.SkillId == skillId);
            SkillsViewModel skillVm = new()
            {
                SkillName = skillObj.SkillName,
                SkillId = skillObj.SkillId
            };
            return PartialView("_EditSkill", skillVm);
        }

        public IActionResult EditSkill(int skillId, string skillName)
        {
            var skillObj = _IUnitOfWork.SkillsRepository.GetFirstOrDefault(s => s.SkillId == skillId);

            skillObj.SkillName = skillName;
            _IUnitOfWork.SkillsRepository.Update(skillObj);
            _IUnitOfWork.Save();
            return Ok(200);
        }

        public IActionResult getSearchedSkills(string? searchText)
        {
            List<Skill> skillList = _IUnitOfWork.SkillsRepository.getSearchedSkillList(searchText);
            if(skillList != null)
            {
                List<SkillsViewModel> skillVmList = new();
                foreach (var skill in skillList)
                {
                    skillVmList.Add(ConvertToSkillVm(skill));
                }
                return PartialView("_MissionSkill", skillVmList);
            }
            else
            {
                var skillsList = _IUnitOfWork.SkillsRepository.GetAll().ToList();
                List<SkillsViewModel> skillVmList = new();

                foreach (var skill in skillsList)
                {
                    skillVmList.Add(ConvertToSkillVm(skill));
                }
                return PartialView("_MissionSkill", skillVmList);

            }
        }

        public IActionResult MissionApplicationList()
        {
           var missionAppList =  _IUnitOfWork.MissionApplicationRepository.getAllMissionApplication();

                List<MissionApplicationViewModel> missionAppVmList = new();
            if (missionAppList != null)
            {
                foreach(var mission in missionAppList)
                {
                    missionAppVmList.Add(ConvertToMissionAppVm(mission));
                }
            }
            return PartialView("_MissionApplication", missionAppVmList);
        }

        private MissionApplicationViewModel ConvertToMissionAppVm(MissionApplication mission)
        {
            MissionApplicationViewModel missionApplicationVm = new()
            {
                MissionApplicationId = mission.MissionApplicationId,
                AppliedAt = mission.AppliedAt,
                ApprovalStatus = mission.ApprovalStatus,
                Mission = mission.Mission,
                User = mission.User,
                UserId = mission.UserId,
                MissionId = mission.MissionId,
            };
            return missionApplicationVm;
        }

        public IActionResult ApproveStatus(int missionAppId)
        {
            var result = _IUnitOfWork.MissionApplicationRepository.ApproveApplication(missionAppId);
            if (result != 0)
            {
                return Ok();
            }
            return null;
        }

        public IActionResult DeclineStatus(int missionAppId)
        {

            var result = _IUnitOfWork.MissionApplicationRepository.DeclineApplication(missionAppId);
            if (result != 0)
            {
                return Ok();
            }
            return null;

        }

        public IActionResult getSearchedMissions(string? searchText)
        {
            List<MissionApplication> missionAppList = _IUnitOfWork.MissionApplicationRepository.getSearchedMissionList(searchText);
            if(missionAppList != null)
            {
                List<MissionApplicationViewModel> missionAppVmList = new();
                foreach (var mission in missionAppList)
                {
                    missionAppVmList.Add(ConvertToMissionAppVm(mission));
                }
                return PartialView("_MissionApplication", missionAppVmList);

            }
            else
            {
                var missionApplicationList = _IUnitOfWork.MissionApplicationRepository.getAllMissionApplication();

                List<MissionApplicationViewModel> missionAppVmList = new();
                
                    foreach (var mission in missionApplicationList)
                    {
                        missionAppVmList.Add(ConvertToMissionAppVm(mission));
                    }
                
                return PartialView("_MissionApplication", missionAppVmList);

            }
        }
    }

    
}

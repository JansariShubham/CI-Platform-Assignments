﻿using CIPlatform.entities.DataModels;
using CIPlatform.entities.ViewModels;
using CIPlatform.repository.IRepository;
using CIPlatform.repository.Repository;
using CIPlatform.utilities;
using CIPlatformWeb.Areas.Users.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.VisualBasic;
using System.Reflection.Metadata.Ecma335;
using HtmlAgilityPack;

namespace CIPlatformWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _IUnitOfWork;

        private readonly EmailSender _EmailSender;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public DashboardController(ILogger<HomeController> logger, IUnitOfWork iUnitOfWork, EmailSender emailSender, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _IUnitOfWork = iUnitOfWork;
            _EmailSender = emailSender;
            _WebHostEnvironment = webHostEnvironment;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home", new {area="Users"});
        }

        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("isAdmin") != "true")
            {
                return RedirectToAction("PlatFormLandingPage", "Home", new { area = "Users" });
            }
            return View();
        }


        public IActionResult getAllUsers()
        {
            var userList = _IUnitOfWork.UserRepository.GetAll().ToList();
            List<UserProfile> userVmList = new();
            foreach (var user in userList)
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

        public IActionResult AddUser(string? firstName, string? lastName, string? email, string? empId, string? department, string? password)
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
            var userObj = _IUnitOfWork.UserRepository.GetFirstOrDefault(u => u.UserId == userId);
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


        public IActionResult editUser(string? firstName, string? lastName, string? email, string? empId, string? department, long? userId)
        {

            var userObj = _IUnitOfWork.UserRepository.GetFirstOrDefault(u => u.UserId == userId);

            userObj.FirstName = firstName;
            userObj.LastName = lastName;
            userObj.Email = email;
            userObj.EmployeeId = empId;
            userObj.Departmemt = department;
           
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
                var isStatusChange = _IUnitOfWork.UserRepository.ChangeUserStatus(userId, 0);
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
            List<User> userList = _IUnitOfWork.UserRepository.getSearchedResult(searchText);
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
                foreach (var cms in cmsList)
                {
                    cmsVmList.Add(ConvertToCmsVm(cms));
                }
            }
            return PartialView("_CMS", cmsVmList);
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

        public IActionResult cmsAddPage()
        {
            return PartialView("_AddCms");
        }

        [HttpPost]
        public IActionResult AddCmsDetails(string? title, string? desc, string? slug, bool status)
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
            if (status == 1)
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
                Title = cmsObj.Title,


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

        [HttpPut]
        public IActionResult StopMission(long missionId)
        {
            int result = _IUnitOfWork.MissionRepository.CloseMission(missionId);
            if(result != 0)
            {

            return Ok();
            }
            return StatusCode(500);
        }

        public IActionResult AddMissionTheme(ThemeViewModel model)
        {
            if (model != null)
            {
                MissionTheme obj = new()
                {
                    Status = model.Status,
                    Title = model.Title,
                    CreatedAt = DateTimeOffset.Now
                };

                _IUnitOfWork.MissionThemeRepository.Add(obj);
                _IUnitOfWork.Save();

            }
            return Ok(200);
        }

        public IActionResult ChangeMissionThemeStatus(int missionThemeId, byte missionThemeStatus)
        {
            if (missionThemeStatus == 1)
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

        public IActionResult EditThemeData(string title, byte status, int themeId)
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
            if (searchedThemeList != null)
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
            var skillList = _IUnitOfWork.SkillsRepository.GetAll().ToList();
            List<SkillsViewModel> skillVmList = new();

            foreach (var skill in skillList)
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
                Status = (bool)skill.Status

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
                SkillName = model.SkillName,
            };
            _IUnitOfWork.SkillsRepository.Add(obj);
            _IUnitOfWork.Save();
            return Ok(200);
        }

        [HttpPost]
        public IActionResult ChangeSkillStatus(bool status, int skillId)
        {
            if (status)
            {
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
            if (skillList != null)
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
            var missionAppList = _IUnitOfWork.MissionApplicationRepository.getAllMissionApplication();

            List<MissionApplicationViewModel> missionAppVmList = new();
            if (missionAppList != null)
            {
                foreach (var mission in missionAppList)
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
            if (missionAppList != null)
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


        public IActionResult getStoryList()
        {
            var storyList = _IUnitOfWork.StoryRepository.getAllStories().ToList();

            List<StoryShareViewModel> storyVmList = new();
            foreach (var story in storyList)
            {
                storyVmList.Add(ConvertToStoryVm(story));
            }
            return PartialView("_Story", storyVmList);
        }

        private StoryShareViewModel ConvertToStoryVm(Story story)
        {
            StoryShareViewModel storyVm = new()
            {
                StoryId = story.StoryId,
                StoryTitle = story.Title,
                User = story.User,
                Mission = story.Mission,
                Status = story.Status
            };
            return storyVm;

        }

        public IActionResult getSearchedStories(string? searchText)
        {
            var storyList = _IUnitOfWork.StoryRepository.getSearchedStories(searchText);
            if (storyList != null)
            {
                List<StoryShareViewModel> storyVmList = new();
                foreach (var story in storyList)
                {
                    storyVmList.Add(ConvertToStoryVm(story));
                }
                return PartialView("_Story", storyVmList);
            }
            else
            {
                var storiesList = _IUnitOfWork.StoryRepository.getAllStories().ToList();
                List<StoryShareViewModel> storyVmList = new();
                foreach (var story in storiesList)
                {
                    storyVmList.Add(ConvertToStoryVm(story));
                }
                return PartialView("_Story", storyVmList);
            }
        }


        public IActionResult ApproveStory(int storyId)
        {
            _IUnitOfWork.StoryRepository.ApproveStoryStatus(storyId);
            return Ok(200);
        }

        public IActionResult DeclineStory(int storyId)
        {
            _IUnitOfWork.StoryRepository.DeclineStoryStatus(storyId);
            return Ok(200);
        }


        public IActionResult StoryDetail(int id)
        {
            var storyObj = _IUnitOfWork.StoryRepository.getAllStories().FirstOrDefault(s => s.StoryId == id);
            StoryListingViewModel storyVm = new()
            {
                StoryId = storyObj.StoryId,
                Description = HtmlToPlainText(storyObj.Description!),
                Title = storyObj.Title,
                StoryViews = storyObj.StoryViews,
                User = storyObj.User,
                imageUrl = getUrl(storyObj.StoryMedia, storyObj.StoryId),

            };
            return View(storyVm);
        }
        public static string HtmlToPlainText(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.DocumentNode.InnerText;
        }

        private string getUrl(ICollection<StoryMedia> storyMedia, int storyId)
        {
            var storyMediaObj = _IUnitOfWork.StoryMediaRepository.GetFirstOrDefault(sm => sm.StoryId == storyId);
            if (storyMediaObj != null)
            {
                var storyName = storyMediaObj.MediaName;
                var storyPath = storyMediaObj.MadiaPath;
                var storyType = storyMediaObj.MediaType;

                var url = storyPath + storyName + storyType;
                return url;
            }
            return null;

        }

        public IActionResult DeleteStory(int storyId)
        {
            var storyMediaList = _IUnitOfWork.StoryMediaRepository.GetAll().Where(sm => sm.StoryId == storyId);

            if (storyMediaList != null)
            {

                string wwwRootPath = _WebHostEnvironment.WebRootPath;
                var path = $@"{wwwRootPath}\images\storyimages";

                foreach (var m in storyMediaList)
                {

                    var fileName = m.MediaName;
                    var filePath = m.MadiaPath;
                    var fileType = m.MediaType;



                    var url = Path.Combine(path, fileName + fileType);
                    System.IO.File.Delete(url);
                    _IUnitOfWork.StoryMediaRepository.RemoveRange(storyMediaList);
                    _IUnitOfWork.Save();

                }
            }
            var storyInviteObj = _IUnitOfWork.StoryInviteRepository.GetAll().Where(si => si.StoryId == storyId);
            if (storyInviteObj != null)
            {
                _IUnitOfWork.StoryInviteRepository.RemoveRange(storyInviteObj);
            }
            var storyObj = _IUnitOfWork.StoryRepository.GetFirstOrDefault(s => s.StoryId == storyId);
            _IUnitOfWork.StoryRepository.Delete(storyObj);
            _IUnitOfWork.Save();
            return Ok(200);

        }

        public IActionResult getBannerList()
        {
            var bannerList = _IUnitOfWork.BannerRepository.GetAll();

            List<BannerViewModel> bannerVmList = new();

            foreach (var banner in bannerList)
            {
                bannerVmList.Add(ConvertToBannerVm(banner));
            }

            return PartialView("_Banner", bannerVmList);
        }

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


        public IActionResult getBannerAddForm()
        {
            return PartialView("_AddBanner");
        }

        public IActionResult AddBannerDetails(IFormFile image, string TextTitle, string TextDesc, int SortOrder, bool Status)
        {

            string wwwRootPath = _WebHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString();
            var path = @"images\banner_images";
            var uploads = Path.Combine(wwwRootPath, path);
            var extension = Path.GetExtension(image?.FileName);
            using (var fileStrems = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                image?.CopyTo(fileStrems);
            }

            Banner obj = new()
            {
                BannerImage = @"\images\banner_images\" + fileName + extension,
                CreatedAt = DateTimeOffset.Now,
                TextDesc = TextDesc,
                TextTitle = TextTitle,
                SortOrder = SortOrder,
                Status = Status
            };

            _IUnitOfWork.BannerRepository.Add(obj);
            _IUnitOfWork.Save();
            return Ok(200);
        }


        public IActionResult ChangeBannerStatus(int bannerId, bool status)
        {
            var result = 0;
            if (status)
            {
                result = _IUnitOfWork.BannerRepository.ChangeStatus(bannerId, false);
            }
            else
            {
                result = _IUnitOfWork.BannerRepository.ChangeStatus(bannerId, true);
            }
            if (result != 0)
            {
                return Ok(200);
            }
            return StatusCode(500);
        }

        public IActionResult getEditBannerForm(int bannerId)
        {
            var bannerObj = _IUnitOfWork.BannerRepository.GetFirstOrDefault(b => b.BannerId == bannerId);
            BannerViewModel bannerVm = new()
            {
                BannerId = bannerObj.BannerId,
                Path = bannerObj.BannerImage,
                SortOrder = bannerObj.SortOrder,
                Status = (bool)bannerObj.Status,
                TextDesc = bannerObj.TextDesc,
                TextTitle = bannerObj.TextTitle,

            };
            return PartialView("_EditBanner", bannerVm);
        }

        public IActionResult EditBannerDetails(IFormFile image, string TextTitle, string TextDesc, int SortOrder, bool Status, int bannerId)
        {
            var bannerObj = _IUnitOfWork.BannerRepository.GetFirstOrDefault(b => b.BannerId == bannerId);


            string wwwRootPath = _WebHostEnvironment.WebRootPath;
            var path = $@"{wwwRootPath}\images\banner_images\";

            var url = Path.Combine(path, Path.GetFileName(bannerObj.BannerImage!));
            System.IO.File.Delete(url);


            string fileName = Guid.NewGuid().ToString();

            var uploads = Path.Combine(wwwRootPath, path);
            var extension = Path.GetExtension(image?.FileName);
            using (var fileStrems = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                image?.CopyTo(fileStrems);
            }

            bannerObj.UpdatedAt = DateTimeOffset.Now;
            bannerObj.TextDesc = TextDesc;
            bannerObj.TextTitle = TextTitle;
            bannerObj.BannerImage = @"\images\banner_images\" + fileName + extension;
            bannerObj.Status = Status;
            bannerObj.SortOrder = SortOrder;

            _IUnitOfWork.BannerRepository.Update(bannerObj);
            _IUnitOfWork.Save();

            return Ok(200);
        }

        public IActionResult getMissionList()
        {
            var missionList = _IUnitOfWork.MissionRepository.GetAll();
            List<PlatformLandingViewModel> missionVmList = new();

            foreach (var mission in missionList)
            {
                missionVmList.Add(ConvertToMissionVm(mission));

            }
            return PartialView("_Mission", missionVmList);
        }

        private PlatformLandingViewModel ConvertToMissionVm(Mission mission)
        {
            PlatformLandingViewModel missionVm = new()
            {
                MissionId = mission.MissionId,
                Title = mission.Title,
                EndDate = mission.EndDate,
                StartDate = mission.StartDate,
                MissionType = mission.MissionType,
                IsActive = (bool)mission.IsActive,
                Status = mission.Status
                
            };
            return missionVm;
        }

        public IActionResult getSearchedMission(string? searchText)
        {
            var missionList = _IUnitOfWork.MissionRepository.GetSearchedMissionList(searchText);
            if (missionList != null)
            {
                List<PlatformLandingViewModel> missionVmList = new();

                foreach (var mission in missionList)
                {
                    missionVmList.Add(ConvertToMissionVm(mission));

                }
                return PartialView("_Mission", missionVmList);

            }
            else
            {
                var missionsList = _IUnitOfWork.MissionRepository.getAllMissions();
                List<PlatformLandingViewModel> missionVmList = new();

                foreach (var mission in missionsList)
                {
                    missionVmList.Add(ConvertToMissionVm(mission));

                }
                return PartialView("_Mission", missionVmList);
            }

        }


        public IActionResult ChangeMissionStatus(int missionId, bool status)
        {
            var result = 0;
            if (status)
            {
                result = _IUnitOfWork.MissionRepository.ChangeMissionStatus(missionId, false);
            }
            else
            {
                result = _IUnitOfWork.MissionRepository.ChangeMissionStatus(missionId, true);
            }
            if (result != 0)
            {
                return Ok(200);
            }
            return StatusCode(500);
        }

        public IActionResult getAddTimeMissionForm()
        {
            var cityList = _IUnitOfWork.CityRepository.GetAll().ToList();
            var countryList = _IUnitOfWork.CountryRepository.GetAll().ToList();

            var themeList = _IUnitOfWork.MissionThemeRepository.GetAll().Where(t => t.Status != 0);
            var skillList = _IUnitOfWork.SkillsRepository.GetAll().Where(s => s.Status == true);

            TimeMissionViewModel timeMissionVm = new()
            {
                CityList = getCityListVm(cityList),
                CountryList = getCountryList(countryList),
                SkillList = getSkillList(skillList),
                ThemeList = getThemeList(themeList)

            };
            return PartialView("_AddTimeMission", timeMissionVm);
        }

        private ICollection<ThemeViewModel> getThemeList(IEnumerable<MissionTheme> themeList)
        {
            List<ThemeViewModel> themeListVm = new();
            foreach (var theme in themeList)
            {
                themeListVm.Add(ConvertToMissionThemeVm(theme));
            }
            return themeListVm;
        }

        private ICollection<SkillsViewModel> getSkillList(IEnumerable<Skill> skillList)
        {
            List<SkillsViewModel> skillListVm = new();
            foreach (var skill in skillList)
            {
                skillListVm.Add(ConvertToSkillVm(skill));
            }
            return skillListVm;
        }

        private ICollection<CountryViewModel> getCountryList(List<Country> countryList)
        {
            List<CountryViewModel> countryVmList = new();
            foreach (var country in countryList)
            {
                countryVmList.Add(ConvertToCountryVm(country));
            }
            return countryVmList;
        }

        private CountryViewModel ConvertToCountryVm(Country country)
        {
            CountryViewModel countryVm = new()
            {
                CountryId = country.CountryId,
                Name = country.Name
            };
            return countryVm;
        }

        private ICollection<CityViewModel> getCityListVm(List<City> cityList)
        {
            List<CityViewModel> cityVmList = new();
            foreach (var city in cityList)
            {
                cityVmList.Add(CovertToCityVm(city));
            }
            return cityVmList;
        }

        private CityViewModel CovertToCityVm(City city)
        {
            CityViewModel cityVm = new()
            {
                CityId = city.CityId,
                Name = city.Name
            };
            return cityVm;
        }

        public IActionResult getAddGoalMissionForm()
        {
            var cityList = _IUnitOfWork.CityRepository.GetAll().ToList();
            var countryList = _IUnitOfWork.CountryRepository.GetAll().ToList();

            var themeList = _IUnitOfWork.MissionThemeRepository.GetAll().Where(t => t.Status != 0);
            var skillList = _IUnitOfWork.SkillsRepository.GetAll().Where(s => s.Status == true);

            GoalMissionViewModel goalMissionVm = new()
            {
                CityList = getCityListVm(cityList),
                CountryList = getCountryList(countryList),
                SkillList = getSkillList(skillList),
                ThemeList = getThemeList(themeList)
            };
            return PartialView("_AddGoalMission", goalMissionVm);
        }


        public IActionResult AddTimeMission(TimeMissionViewModel model, List<int> MissionSkills)
        {
            Mission missionObj = new()
            {
                Title = model.Title,
                Description = model.Description,
                ShortDesc = model.ShortDesc,
                Availability = model.Availability,
                CityId = model.CityId,
                CountryId = model.CountryId,
                CreatedAt = DateTimeOffset.Now,
                EndDate = model.EndDate,
                StartDate = model.StartDate,
                IsActive = (bool)model.IsActive!,
                TotalSeats = model.TotalSeats,
                RegDeadline = model.RegDeadline,
                ThemeId = model.ThemeId,
                OrgDetails = model.OrgDetail,
                OrgName = model.OrgDetail,
                MissionType = true,
                Status = true

            };
            _IUnitOfWork.MissionRepository.Add(missionObj);
            _IUnitOfWork.Save();
            SaveMissionSkill(MissionSkills, missionObj.MissionId);
            SaveMissionMedia(model.Images, missionObj.MissionId);
            SaveMissionDocuments(model.Documents, missionObj.MissionId);


            return Ok(200);
        }

        public IActionResult AddGoalMission(GoalMissionViewModel model, List<int> MissionSkills)
        {
        Mission missionObj = new()
        {
            Title = model.Title,
            Description = model.Description,
            ShortDesc = model.ShortDesc,
            Availability = model.Availability,
            CityId = model.CityId,
            CountryId = model.CountryId,
            CreatedAt = DateTimeOffset.Now,
            EndDate = model.EndDate,
            StartDate = model.StartDate,
            IsActive = (bool)model.IsActive!,
            TotalSeats = model.TotalSeats,
            RegDeadline = model.RegDeadline,
            ThemeId = model.ThemeId,
            OrgDetails = model.OrgDetail,
            OrgName = model.OrgDetail,
            MissionType = false,
            Status = true
            };
            _IUnitOfWork.MissionRepository.Add(missionObj);
            _IUnitOfWork.Save();
            SaveGoalMission(missionObj.MissionId, model.GoalValue, model.GoalObjectiveText);
            SaveMissionMedia(model.Images, missionObj.MissionId);
            SaveMissionDocuments(model.Documents, missionObj.MissionId);


            return Ok(200);
        }

        private void SaveGoalMission(long missionId, int? goalValue, string? goalObjectiveText)
        {
            GoalMission goalMissionObj = new()
            {
                CreatedAt = DateTimeOffset.Now,
                MissionId = missionId,
                GoalObjectiveText = goalObjectiveText,
                GoalValue = (int)goalValue!,
            };
            _IUnitOfWork.GoalMissionRepository.Add(goalMissionObj);
            _IUnitOfWork.Save();

        }


        private void SaveMissionDocuments(List<IFormFile>? documents, long missionId)
        {
            if (documents != null)
            {
                string wwwRootPath = _WebHostEnvironment.WebRootPath;
                foreach (var f in documents)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"documents");
                    var extension = Path.GetExtension(f?.FileName);

                    using (var fileStrems = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        f?.CopyTo(fileStrems);
                    }



                    MissionDocument missionDocObj = new()
                    {
                        MissionId = missionId,
                        DocumentPath = @"\images\documents\",
                        DocumentName = fileName,
                        DocumentType = extension!,
                        CreatedAt = DateTimeOffset.Now,
                    };

                    _IUnitOfWork.MissionDocRepository.Add(missionDocObj);
                    _IUnitOfWork.Save();
                }
            }
        }

        private void SaveMissionMedia(List<IFormFile>? images, long missionId)
        {
            if (images != null)
            {
                string wwwRootPath = _WebHostEnvironment.WebRootPath;
                foreach (var f in images)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\mission_images");
                    var extension = Path.GetExtension(f?.FileName);

                    using (var fileStrems = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        f?.CopyTo(fileStrems);
                    }



                    MissionMedia missionMediaObj = new()
                    {
                        MissionId = missionId,
                        MediaPath = @"\images\mission_images\",
                        MediaName = fileName,
                        MediaType = extension!,
                        CreatedAt = DateTimeOffset.Now,
                        DefaultMedia = true,
                    };

                    _IUnitOfWork.MissionMediaRepository.Add(missionMediaObj);
                    _IUnitOfWork.Save();
                }
            }
        }
        public void SaveMissionSkill(List<int> MissionSkills, long missionId)
        {

            foreach (var skill in MissionSkills)
            {
                MissionSkill skillObj = new();
                skillObj.MissionId = missionId;
                skillObj.SkillId = skill;
                skillObj.CreatedAt = DateTimeOffset.Now;
                _IUnitOfWork.MissionSkillRepository.Add(skillObj);
                _IUnitOfWork.Save();
            }
        }


        public IActionResult GetTimeMissionEditForm(long missionId)
        {
            var missionObj = _IUnitOfWork.MissionRepository.GetMissionById(missionId);
            var cityList = _IUnitOfWork.CityRepository.GetAll().ToList();
            var countryList = _IUnitOfWork.CountryRepository.GetAll().ToList();

            var themeList = _IUnitOfWork.MissionThemeRepository.GetAll().Where(t => t.Status != 0);
            var skillList = _IUnitOfWork.SkillsRepository.GetAll().Where(s => s.Status == true);
            TimeMissionViewModel timeMissionVm = new()
            {
                MissionId = missionId,
                Availability = missionObj.Availability,
                CityList = getCityListVm(cityList),
                CountryList = getCountryList(countryList),
                ThemeList = getThemeList(themeList),
                SkillList = getSkillList(skillList),
                Description = missionObj.Description,
                TotalSeats = missionObj.TotalSeats,
                MissionDocuments = ConvertToMissionDocVm(missionObj.MissionDocuments),
                MissionMedia = ConvertToMissionMedia(missionObj.MissionMedia),
                CityId = missionObj.CityId,
                CountryId = missionObj.CountryId,
                MissionSkills = ConvertToMissionSkillVm(missionObj.MissionSkills),
                EndDate = missionObj.EndDate,
                IsActive = missionObj.IsActive,
                StartDate = missionObj.StartDate,
                ShortDesc = missionObj.ShortDesc,
                OrgDetail = missionObj.OrgDetails,
                OrgName = missionObj.OrgName,
                ThemeId = missionObj.ThemeId,
                Title = missionObj.Title,
                RegDeadline = missionObj.RegDeadline,
                

            };


            return PartialView("_EditTimeMission", timeMissionVm);

        }

        private List<MissionSkillViewModel> ConvertToMissionSkillVm(ICollection<MissionSkill> missionSkills)
        {
            return missionSkills.Select(ms => new MissionSkillViewModel()
            {
                SkillId = ms.SkillId,
                SkillName = ms.Skill.SkillName

            }).ToList();

        }

        private ICollection<MissionMediaViewModel> ConvertToMissionMedia(ICollection<MissionMedia> missionMedia)
        {
            return missionMedia.Select(m => new MissionMediaViewModel()
            {
                DefaultMedia = m.DefaultMedia,
                MediaName = m.MediaName,
                MediaPath = m.MediaPath,
                MediaType = m.MediaType

            }).ToList();
        }

        private List<MissionDocumentVM> ConvertToMissionDocVm(ICollection<MissionDocument> missionDocuments)
        {
            return missionDocuments.Select(md => new MissionDocumentVM()
            {
                DocumentName = md.DocumentName,
                DocumentPath = md.DocumentPath,
                DocumentType = md.DocumentType
            }).ToList();



        }

        public IActionResult EditTimeMissionDetails(TimeMissionViewModel model, List<int> MissionSkills, List<int> preloadedMissionSkill)
        {
            var missionObj = _IUnitOfWork.MissionRepository.GetFirstOrDefault(m => m.MissionId == model.MissionId);
            missionObj.Availability = model.Availability;
            missionObj.Title = model.Title;
            missionObj.Description = model.Description;
            missionObj.CountryId = model.CountryId;
            missionObj.CityId = model.CityId;
            missionObj.ShortDesc = model.ShortDesc;
            missionObj.IsActive = (bool)model.IsActive;
            missionObj.OrgDetails = model.OrgDetail;
            missionObj.OrgName = model.OrgName;
            missionObj.ThemeId = model.ThemeId;
            missionObj.StartDate = model.StartDate;
            missionObj.EndDate = model.EndDate;
            missionObj.RegDeadline = model.RegDeadline;
            missionObj.TotalSeats = model.TotalSeats;

            if (missionObj != null)
            {
                _IUnitOfWork.MissionRepository.Update(missionObj);
                _IUnitOfWork.Save();

            }


            EditMissionMedia(model.Images, model.MissionId);
            EditMissionDocuments(model.Documents, model.MissionId);
            EditMissionSkills(model.MissionId, MissionSkills, preloadedMissionSkill);
            return Ok();
        }

        private void EditMissionSkills(long? missionId, List<int> missionSkills, List<int> preloaded )
        {
            // var skillObj =  _IUnitOfWork.MissionSkillRepository.GetFirstOrDefault(m => m.MissionId == missionId);
            ////var skillObjList = _IUnitOfWork.MissionSkillRepository.GetAll().Where(ms => ms.MissionId == missionId);
            //  if (skillObj != null)
            //  {
            //      foreach (var skill in missionSkills)
            //      {
            //          skillObj.SkillId = skill;
            //          skillObj.UpdatedAt = DateTimeOffset.Now;
            //          _IUnitOfWork.MissionSkillRepository.Update(skillObj);
            //      }
            //      _IUnitOfWork.Save();
            //  }
            var skillObjList = _IUnitOfWork.MissionSkillRepository.GetAll().Where(ms => ms.MissionId == missionId);
            if(skillObjList != null)
            {
                _IUnitOfWork.MissionSkillRepository.RemoveRange(skillObjList);

                foreach (var skill in missionSkills)
                {
                    MissionSkill skillObj = new();
                    skillObj.MissionId = (long)missionId; 
                    skillObj.SkillId = skill;
                    skillObj.UpdatedAt = DateTimeOffset.Now;
                    _IUnitOfWork.MissionSkillRepository.Add(skillObj);
                }
                _IUnitOfWork.Save();
            }



        }

        private void EditMissionDocuments(List<IFormFile>? documents, long? missionId)
        {
            if (documents != null)
            {
                var media = _IUnitOfWork.MissionDocRepository.GetAll().Where(m => m.MissionId == missionId);
                string wwwRootPath = _WebHostEnvironment.WebRootPath;

                if (media != null)
                {

                    var path = $@"{wwwRootPath}\documents";

                    foreach (var m in media)
                    {

                        var fileName = m.DocumentName;
                        var filePath = m.DocumentPath;
                        var fileType = m.DocumentType;



                        var url = Path.Combine(path, fileName + fileType);
                        System.IO.File.Delete(url);
                        _IUnitOfWork.MissionDocRepository.RemoveRange(media);
                        _IUnitOfWork.Save();


                    }
                }
                foreach (var f in documents)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"documents");
                    var extension = Path.GetExtension(f?.FileName);

                    using (var fileStrems = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        f?.CopyTo(fileStrems);
                    }

                    MissionDocument missionDocObj = new()
                    {
                        MissionId = missionId,
                        DocumentPath = @"\images\documents\",
                        DocumentName = fileName,
                        DocumentType = extension,

                    };


                    _IUnitOfWork.MissionDocRepository.Add(missionDocObj);
                    _IUnitOfWork.Save();
                }
            }
        }

        private void EditMissionMedia(List<IFormFile>? images, long? missionId)
        {
            if (images != null)
            {
                var media = _IUnitOfWork.MissionMediaRepository.GetAll().Where(m => m.MissionId == missionId);
                string wwwRootPath = _WebHostEnvironment.WebRootPath;

                if (media != null)
                {

                    var path = $@"{wwwRootPath}\images\mission_images";

                    foreach (var m in media)
                    {

                        var fileName = m.MediaName;
                        var filePath = m.MediaPath;
                        var fileType = m.MediaType;



                        var url = Path.Combine(path, fileName + fileType);
                        System.IO.File.Delete(url);
                        _IUnitOfWork.MissionMediaRepository.RemoveRange(media);
                        _IUnitOfWork.Save();

                    }
                }
                foreach (var f in images)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\mission_images");
                    var extension = Path.GetExtension(f?.FileName);

                    using (var fileStrems = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        f?.CopyTo(fileStrems);
                    }



                    MissionMedia missionMediaObj = new()
                    {
                        MissionId = missionId,
                        MediaPath = @"\images\mission_images\",
                        MediaName = fileName,
                        MediaType = extension!,
                        CreatedAt = DateTimeOffset.Now,
                        DefaultMedia = true,
                    };

                    _IUnitOfWork.MissionMediaRepository.Add(missionMediaObj);
                    _IUnitOfWork.Save();
                }
            }
        }

            

            public IActionResult GetGoalMissionEditForm(long missionId)
            {

            var missionObj = _IUnitOfWork.MissionRepository.GetMissionById(missionId);
            var cityList = _IUnitOfWork.CityRepository.GetAll().ToList();
            var countryList = _IUnitOfWork.CountryRepository.GetAll().ToList();
            var goalMissionObj = _IUnitOfWork.GoalMissionRepository.GetFirstOrDefault(gm => gm.MissionId == missionId);

            var themeList = _IUnitOfWork.MissionThemeRepository.GetAll().Where(t => t.Status != 0);
            var skillList = _IUnitOfWork.SkillsRepository.GetAll().Where(s => s.Status == true);
            GoalMissionViewModel goalMissionVm = new()
            {
                MissionId = missionId,
                Availability = missionObj.Availability,
                CityList = getCityListVm(cityList),
                CountryList = getCountryList(countryList),
                ThemeList = getThemeList(themeList),
                SkillList = getSkillList(skillList),
                Description = missionObj.Description,
                TotalSeats = missionObj.TotalSeats,
                MissionDocuments = ConvertToMissionDocVm(missionObj.MissionDocuments),
                MissionMedia = ConvertToMissionMedia(missionObj.MissionMedia),
                CityId = missionObj.CityId,
                CountryId = missionObj.CountryId,
                MissionSkills = ConvertToMissionSkillVm(missionObj.MissionSkills),
                EndDate = missionObj.EndDate,
                IsActive = missionObj.IsActive,
                StartDate = missionObj.StartDate,
                ShortDesc = missionObj.ShortDesc,
                OrgDetail = missionObj.OrgDetails,
                OrgName = missionObj.OrgName,
                ThemeId = missionObj.ThemeId,
                Title = missionObj.Title,
                RegDeadline = missionObj.RegDeadline,
                GoalObjectiveText = goalMissionObj.GoalObjectiveText,
                GoalValue = goalMissionObj.GoalValue,
                

            };


            return PartialView("_EditGoalMission", goalMissionVm);
            
            }

        public IActionResult EditGoalMissionDetails(GoalMissionViewModel model, List<int> MissionSkills, List<int> preloadedMissionSkill)
        {

            var missionObj = _IUnitOfWork.MissionRepository.GetFirstOrDefault(m => m.MissionId == model.MissionId);
            missionObj.Availability = model.Availability;
            missionObj.Title = model.Title!;
            missionObj.Description = model.Description;
            missionObj.CountryId = model.CountryId;
            missionObj.CityId = model.CityId;
            missionObj.ShortDesc = model.ShortDesc!;
            missionObj.IsActive = (bool)model.IsActive!;
            missionObj.OrgDetails = model.OrgDetail;
            missionObj.OrgName = model.OrgName;
            missionObj.ThemeId = model.ThemeId;
            missionObj.StartDate = model.StartDate;
            missionObj.EndDate = model.EndDate;
            missionObj.RegDeadline = model.RegDeadline;
            missionObj.TotalSeats = model.TotalSeats;

            if (missionObj != null)
            {
                _IUnitOfWork.MissionRepository.Update(missionObj);
                _IUnitOfWork.Save();

            }
            var goalMissionObj = _IUnitOfWork.GoalMissionRepository.GetFirstOrDefault(gm => gm.MissionId == model.MissionId);
            if (goalMissionObj != null)
            {
                goalMissionObj.GoalObjectiveText = model.GoalObjectiveText;
                goalMissionObj.GoalValue = (int)model.GoalValue;
                goalMissionObj.UpdatedAt = DateTimeOffset.Now;
                _IUnitOfWork.GoalMissionRepository.Update(goalMissionObj);
                _IUnitOfWork.Save();

            }
            
            EditMissionMedia(model.Images, model.MissionId);
            EditMissionDocuments(model.Documents, model.MissionId);
            EditMissionSkills(model.MissionId, MissionSkills, preloadedMissionSkill);
            return Ok();
        }
        [HttpGet]
        public JsonResult GetCitiesByCountry(int country)
        {
            var CountryObj = _IUnitOfWork.CountryRepository.GetFirstOrDefault(countryName => countryName.CountryId == country);
            var cityList = _IUnitOfWork.CityRepository.GetAll().Where(m => m.CountryId == CountryObj.CountryId).ToList();

            return Json(cityList);
        }


    }
}









using CIPlatform.entities.DataModels;
using CIPlatform.entities.ViewModels;
using CIPlatform.repository.IRepository;
using CIPlatform.utilities;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Routing.Constraints;

namespace CIPlatformWeb.Areas.Users.Controllers
{
    [Area("Users")]
    public class StoryController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _IUnitOfWork;
        private readonly EmailSender _EmailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public StoryController(IUnitOfWork IUnitOfWork, EmailSender emailSender, IWebHostEnvironment webHostEnvironment)
        {
            _IUnitOfWork = IUnitOfWork;
            _EmailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult ShareStory(string? id)
        {
            var missionList = _IUnitOfWork.MissionApplicationRepository.getAllMissionApplication();
            var missions = missionList.Where(ma => ma.UserId == long.Parse(id!) && ma.ApprovalStatus == 1).Select(ma => ma.Mission);
            List<PlatformLandingViewModel> missionVm = new();
            foreach (var mission in missions)
            {
                missionVm.Add(ConvertToMissionVm(mission));
            }



            ViewBag.missions = missionVm;
            return View();
        }

        private PlatformLandingViewModel ConvertToMissionVm(Mission? mission)
        {
            PlatformLandingViewModel missionVm = new()
            {
                Title = mission.Title,
                MissionId = mission.MissionId
                
            };
            return missionVm;

        }

        [HttpPost]
        public void AddStory(StoryShareViewModel storyVm, List<IFormFile?> file, string action)
        {
            if (ModelState.IsValid)
            {
                if (storyVm != null)
                {
                    Story storyObj = new()
                    {
                        MissionId = storyVm.MissionId,
                        Title = storyVm.StoryTitle,
                        Description = storyVm.Description,
                        UserId = storyVm.UserId,
                        


                    };
                    var isDraft = action == "draft" ? storyObj.Status = 2 : storyObj.Status = 1;
                    _IUnitOfWork.StoryRepository.Add(storyObj);
                    _IUnitOfWork.Save();
                }
               var storyId = _IUnitOfWork.StoryRepository.GetAll().OrderByDescending(s => s.CreatedAt).FirstOrDefault(s => s.MissionId == storyVm!.MissionId && s.UserId == storyVm.UserId)!.StoryId;
               


                

                if (file != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    foreach (var f in file)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"images\storyimages");
                        var extension = Path.GetExtension(f?.FileName);

                        using (var fileStrems = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            f?.CopyTo(fileStrems);
                        }


                        StoryMedia storyMediaObj = new()
                        {
                            MadiaPath = @"\images\storyimages\",
                            MediaName = fileName,
                            MediaType = extension!,
                            StoryId  = storyId

                        };
                        _IUnitOfWork.StoryMediaRepository.Add(storyMediaObj);
                        _IUnitOfWork.Save();
                    }
                   


                }


            }
        }

        public IActionResult StoryListing()
        {
            var storyList = _IUnitOfWork.StoryRepository.getAllStories().Where(s => s.Status != 2);

            List<StoryListingViewModel> storyListingVm = new();
            foreach (var story in storyList)
            {
                storyListingVm.Add(convertToStoryListingVm(story));
            }
            ViewBag.storyCount = storyListingVm.Count();
            
            return View(storyListingVm);
        }

        private StoryListingViewModel convertToStoryListingVm(Story story)
        {
            StoryListingViewModel vm = new() {
                Title = story.Title,
                Description = HtmlToPlainText(story.Description!),
                User = story.User,
                StoryId = story.StoryId,
                imageUrl = getUrl(story.StoryMedia, story.StoryId),
                MissionId = story.MissionId
                
            };
            return vm;

        }

        private string getUrl(ICollection<StoryMedia> storyMedia, int storyId)
        {
            var storyMediaObj = _IUnitOfWork.StoryMediaRepository.GetFirstOrDefault(sm => sm.StoryId == storyId);
            if (storyMediaObj != null) {
                var storyName = storyMediaObj.MediaName;
                var storyPath = storyMediaObj.MadiaPath;
                var storyType = storyMediaObj.MediaType;

                var url = storyPath  + storyName + storyType;
                return url;
            }
            return null;

        }

        

    public static string HtmlToPlainText(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        return doc.DocumentNode.InnerText;
    }
    [HttpGet]
    public IActionResult GetStories(int pageNum)
     {
            var  storyObj = _IUnitOfWork.StoryRepository.getAllStories().Where(s => s.Status != 2);
           List<StoryListingViewModel> storyListingVm = storyObj.Select(s => convertToStoryListingVm(s)).ToList();
            var stories = storyListingVm.Skip((pageNum - 1) * 1).Take(1).ToList();
            return PartialView("_StoryListing", stories);

     }

    public IActionResult StoryDetail(int? id)
        {
            if (id != 0 && id != null)
            {
                var story = _IUnitOfWork.StoryRepository.getAllStories().FirstOrDefault(s => s.StoryId == id);
                var storyVm = convertToStoryListingVm(story!);
                return View(storyVm);
            }
            return View();
        }

        public void AddUsersToStoryInvite(int[]? usersIdList, int? storyId, int? currentUserId)
        {
            try
            {
                var currentUser = _IUnitOfWork.UserRepository.GetFirstOrDefault(u => u.UserId == currentUserId);
                /*String subject = "Your Friend" + currentUser.FirstName + "is Recommended you this mission!";
                String htmlMessage = "Please Checkout this Mission!";*/
                var url = Url.Action("StoryDetail", "Story", new { id = storyId }, "https");
                string subject = "CI Platform - Story Recommendation";
                string link = $"<a href='{url}' style='text-decoration:none;display:block;width:max-content;border:1px solid black;border-radius:5rem;padding:0.75rem 1rem;margin:1rem auto;color:black;font-size:1rem;'>Open Story</a>";
                string htmlMessage = $"<p style='text-align:center;font-size:2rem'>Your co-worker {currentUser.FirstName} has recommended a story to you.</p><p style='text-align:center;font-size:1.5rem'>Click on the link below check story out</p><hr/>{link}";
                for (int i = 0; i < usersIdList!.Length; i++)
                {

                    StoryInvite storyInviteObj = new()
                    {
                        FromUserId = currentUserId,
                        StoryId = storyId,
                        ToUserId = usersIdList[i],
                        CreatedAt = DateTimeOffset.Now
                    };


                    _IUnitOfWork.StoryInviteRepository.Add(storyInviteObj);
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
    }

}

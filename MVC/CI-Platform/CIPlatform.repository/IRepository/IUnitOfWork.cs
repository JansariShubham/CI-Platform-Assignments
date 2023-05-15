using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.IRepository
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IPasswordResetRepo PasswordResetRepo { get; }

        IMissionRepository MissionRepository { get; }

        IfavMissionRepository FavMissionRepository { get; }
        ICityRepository CityRepository { get; } 

        ICountryRepository CountryRepository { get; }
         ISkillsRepository SkillsRepository { get; }
        IMissionThemeRepository MissionThemeRepository { get; }

        IMissionRatingRepository MissionRatingRepository { get; }

        ICommentRepository CommentRepository { get; }

        IMissionInviteRepository MissionInviteRepository { get; }

        IMissionApplicationRepository MissionApplicationRepository { get; }

        IStoryRepository StoryRepository { get; }

        IStoryMediaRepository StoryMediaRepository { get; }

        IStoryInviteRepository StoryInviteRepository { get; }

        IContactUsRepository ContactUsRepository { get; }
        ITimeSheetRepository TimeSheetRepository { get; }

        IUserSkillRepository UserSkillRepository { get; }

        ICmsRepository CmsRepository { get; }

        IBannerRepository BannerRepository { get; }

        IMissionSkillRepository MissionSkillRepository { get; }

        IMissionMediaRepository MissionMediaRepository { get; }

        IMissionDocRepository MissionDocRepository { get; }

        IGoalMissionRepository GoalMissionRepository { get; }

        INotificationRepository NotificationRepository { get; }

        INotificationSettingRepository NotificationSettingRepository { get; }

        IUserNotificationRepository UserNotificationRepository { get; }

        ILastCheckedRepository LastCheckedRepository { get; }   
        void Save();
    }
}

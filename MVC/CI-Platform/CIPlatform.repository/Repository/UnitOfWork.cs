using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository UserRepository { get; private set; }

        public  IPasswordResetRepo PasswordResetRepo { get; private set; }  

        public IMissionRepository MissionRepository { get; private set; }

        public ICityRepository CityRepository{ get; private set; }

        public ICountryRepository CountryRepository { get; private set; }

        public ISkillsRepository SkillsRepository { get; private set; }

        public IMissionThemeRepository MissionThemeRepository { get; private set; }

        public IfavMissionRepository FavMissionRepository { get; private set; }

        public IMissionRatingRepository MissionRatingRepository { get; private set; }

        public ICommentRepository CommentRepository { get; private set; }

        public IMissionInviteRepository MissionInviteRepository { get; private set; }

        public IMissionApplicationRepository MissionApplicationRepository { get; private set; }
        public IStoryRepository StoryRepository { get; private set; }

        public IStoryMediaRepository StoryMediaRepository { get; private set; }

        public IStoryInviteRepository StoryInviteRepository { get; private set; } 
        
        public IContactUsRepository ContactUsRepository { get; private set; }

        public ITimeSheetRepository TimeSheetRepository { get; private set; }
        private AppDbContext _appDbContext;

        public UnitOfWork(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
            UserRepository = new UserRepository(_appDbContext);
            PasswordResetRepo = new PasswordResetRepo(_appDbContext);
            MissionRepository = new MissionRepository(_appDbContext);
            CityRepository = new CityRepository(_appDbContext);
            CountryRepository = new CountryRepository(_appDbContext);
            SkillsRepository = new SkillsRepository(_appDbContext);
            MissionThemeRepository =  new MissionThemeRepository(_appDbContext);
            FavMissionRepository = new FavMissionRepository(appDbContext);
            MissionRatingRepository = new MissionRatingRepository(appDbContext);
            CommentRepository = new CommentRepository(appDbContext);
            MissionInviteRepository = new MissionInviteRepository(appDbContext);
            MissionApplicationRepository = new MissionApplicationRepository(appDbContext);
            StoryRepository = new StoryRepository(appDbContext);
            StoryMediaRepository = new StoryMediaRepository(appDbContext);
            StoryInviteRepository = new StoryInviteRepository(appDbContext);
            ContactUsRepository = new ContactUsRepository(appDbContext);
            TimeSheetRepository = new TimeSheetRepository(appDbContext);
        }


        public void Save()
        {
            _appDbContext.SaveChanges();
        }

    }
}

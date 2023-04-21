using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class MissionThemeRepository : Repository<MissionTheme>, IMissionThemeRepository
    {

        private AppDbContext _appDbContext;

        public MissionThemeRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public int ChangeThemeStatus(int missionThemeId, byte missionThemeStatus)
        {
            var query = "update mission_theme set status = {0} where mission_theme_id = {1}";

            return _appDbContext.Database.ExecuteSqlRaw(query,  missionThemeStatus, missionThemeId);
             
        }

        public List<MissionTheme> getSearchedThemeList(string searchText)
        {
            if (searchText != null && searchText != "")
            {
               return _appDbContext.MissionThemes.Where(mt => mt.Title.ToLower().Contains(searchText!.ToLower())).ToList();
            }
            return null!;
        }

        public void Update(MissionTheme missionTheme)
        {
            _appDbContext.MissionThemes.Update(missionTheme);
        }
    }
}

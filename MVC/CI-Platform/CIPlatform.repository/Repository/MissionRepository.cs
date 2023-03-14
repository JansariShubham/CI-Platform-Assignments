using CIPlatform.entities.DataModels;
using CIPlatform.entities.ViewModels;
using CIPlatform.repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class MissionRepository: Repository<Mission>, IMissionRepository
    {
        private AppDbContext _appDbContext;

        public MissionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public List<Mission> getAllMissions()
        {
            var missionList = _appDbContext.Missions.Include(mission => mission.MissionApplications)
                 .Include(missions => missions.GoalMissions)
                 .Include(missions => missions.MissionMedia)
                 .Include(missions => missions.FavouriteMissions)
                 .Include(missions => missions.MissionSkills)
                 .Include(missions => missions.Theme)
                 .Include(missions => missions.City)
                 .Include(missions => missions.Country).ToList();
            
            return missionList;
        }



        public List<Mission> getFilters(Filters obj)
        {
            var mission = getAllMissions();
            //var mission = _appDbContext.Missions.ToList();
            StringComparison comp = StringComparison.OrdinalIgnoreCase;
            if (obj.SearchText != "" && obj.SearchText != null)
            {
                mission = mission.Where(m => m.Title.Contains(obj.SearchText, comp)).ToList();
                Console.WriteLine(mission.Count);
            }
            if (obj.Countries.Count() > 0)
            {
                mission = mission.Where(m => obj.Countries.ToList().Contains((int)m.CountryId)).ToList();
            }

            if(obj.Themes.Count() > 0)
            {
                mission = mission.Where(m => obj.Themes.ToList().Contains((int)m.ThemeId)).ToList(); 

            }
            if (obj.Cties.Count() > 0)
            {
                mission = mission.Where(m => obj.Cties.ToList().Contains((int)m.CityId)).ToList();
            }

            return mission;
                    

            
        }
    }
}

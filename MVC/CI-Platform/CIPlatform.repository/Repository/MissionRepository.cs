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
            var filterMission = mission;
            //var mission = _appDbContext.Missions.ToList();
            StringComparison comp = StringComparison.OrdinalIgnoreCase;
            

            if (obj.SearchText != "" && obj.SearchText != null)
            {
                filterMission = filterMission.Where(m => m.Title.Contains(obj.SearchText, comp)).ToList();
                //Console.WriteLine(mission.Count);
            }
            
            if (obj.Countries.Count() > 0)
            {
                filterMission = filterMission.Where(m => obj.Countries.Contains((int)m.CountryId)).ToList();
                //mission = filterMission;
            }

            if(obj.Themes.Count() > 0)
            {
                filterMission = filterMission.Where(m => obj.Themes.Contains((int)m.ThemeId)).ToList();
                //mission = filterMission;
            }
            if (obj.Cties.Count() > 0)
            {
               filterMission = filterMission.Where(m => obj.Cties.Contains((int)m.CityId)).ToList();
               // mission = filterMission;
            }
            if(obj.Skills.Count() > 0)
            {
                filterMission = filterMission.Where(m => 
                   m.MissionSkills.Any(ms => obj.Skills.Any(s => s == ms.SkillId))
                 ).ToList();
               // mission = filterMission;
                //var missionSkillObj = _appDbContext.MissionSkills.Where(m =>obj.Skills.ToList().Contains((int)m.MissionSkillId));

                //mission = mission.Where(m => obj.Skills.Any(s => m.MissionSkills.Any(ms => ms.MissionSkillId == s))).ToList();
            }

            return filterMission.Count() == 0 ? mission : filterMission;
                    

            
        }
    }
}

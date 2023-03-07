using CIPlatform.entities.DataModels;
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
           var missionList =  _appDbContext.Missions.Include(mission => mission.MissionApplications)
                .Include(missions => missions.GoalMissions)
                .Include(missions => missions.MissionMedia)
                .Include(missions => missions.FavouriteMissions)
                .Include(missions => missions.MissionSkills)
                .Include(missions => missions.Theme).ToList();
            
            return missionList;
        }
    }
}

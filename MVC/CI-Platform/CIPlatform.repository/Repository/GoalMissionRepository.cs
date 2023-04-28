using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class GoalMissionRepository : Repository<GoalMission>, IGoalMissionRepository
    {
        private AppDbContext _appDbContext;
        public GoalMissionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Update(GoalMission goalMission)
        {
            _appDbContext.GoalMissions.Update(goalMission);
        }
    }
}


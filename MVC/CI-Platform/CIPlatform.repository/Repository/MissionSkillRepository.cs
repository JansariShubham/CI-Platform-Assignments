using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class MissionSkillRepository : Repository<MissionSkill>, IMissionSkillRepository
    {
        private AppDbContext _appDbContext;
        public MissionSkillRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Update(MissionSkill entity)
        {
            _appDbContext.MissionSkills.Update(entity);
        }
    }
}

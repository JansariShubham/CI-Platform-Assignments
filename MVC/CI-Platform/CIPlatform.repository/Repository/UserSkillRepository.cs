using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class UserSkillRepository :  Repository<UserSkill>, IUserSkillRepository
    {
        private AppDbContext _appDbContext;
        public UserSkillRepository(AppDbContext appDbContext) : base(appDbContext)
        {

            _appDbContext = appDbContext;
        }

        public void Update(UserSkill userSkill)
        {
            _appDbContext.UserSkills.Update(userSkill);
        }
    }
}

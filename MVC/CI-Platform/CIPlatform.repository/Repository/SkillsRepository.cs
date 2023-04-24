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
    public class SkillsRepository: Repository<Skill>, ISkillsRepository
    {
        private AppDbContext _appDbContext;

        public SkillsRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public int ChangeSkillsStatus(int status, int skillId)
        {
            string query = "UPDATE skill SET status = {0} WHERE skill_id = {1}";
            return _appDbContext.Database.ExecuteSqlRaw(query, status, skillId);
        }

        public List<Skill> getSearchedSkillList(string? searchText)
        {
            if (searchText != null && searchText != "")
            {
                return _appDbContext.Skills.Where(s => s.SkillName.ToLower().Contains(searchText!.ToLower())).ToList();
            }
            return null!;
        }

        public void Update(Skill skill)
        {
            _appDbContext.Skills.Update(skill);
        }
    }
}

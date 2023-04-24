using CIPlatform.entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.IRepository
{
    public interface ISkillsRepository : IRepository<Skill>
    {
        public int ChangeSkillsStatus(int status, int skillId);

        public void Update(Skill skill);

        public List<Skill> getSearchedSkillList(string? searchText);
    }
}

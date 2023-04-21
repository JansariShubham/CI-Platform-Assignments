using CIPlatform.entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.IRepository
{
    public interface IMissionThemeRepository : IRepository<MissionTheme>
    {
        public int ChangeThemeStatus(int missionThemeId, byte missionThemeStatus);

        public void Update(MissionTheme missionTheme);

        public List<MissionTheme> getSearchedThemeList(string searchText);
    }
}

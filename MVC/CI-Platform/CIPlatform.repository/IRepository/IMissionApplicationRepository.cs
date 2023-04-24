using CIPlatform.entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.IRepository
{
    public interface IMissionApplicationRepository : IRepository<MissionApplication>
    {
        public List<MissionApplication> getAllMissionApplication();

        public int ApproveApplication(int missionAppId);

        public int DeclineApplication(int missionAppId);

        public List<MissionApplication> getSearchedMissionList(string? searchText);
    }
}

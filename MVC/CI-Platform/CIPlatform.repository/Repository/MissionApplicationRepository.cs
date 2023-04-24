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
    public class MissionApplicationRepository : Repository<MissionApplication>, IMissionApplicationRepository
    {
        private AppDbContext _appDbContext;
        public MissionApplicationRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public int ApproveApplication(int missionAppId)
        {
            string query = "UPDATE mission_application SET approval_status = {0} WHERE mission_application_id = {1}";
           return  _appDbContext.Database.ExecuteSqlRaw(query, 1, missionAppId);
        }

        public int DeclineApplication(int missionAppId)
        {

            string query = "UPDATE mission_application SET approval_status = {0} WHERE mission_application_id = {1}";
            return _appDbContext.Database.ExecuteSqlRaw(query, 0, missionAppId);
        }

        public List<MissionApplication> getAllMissionApplication()
        {
            return _appDbContext.MissionApplications.Include(ma => ma.Mission)
                .Include(ma => ma.User).ToList();
        }

        public List<MissionApplication> getSearchedMissionList(string? searchText)
        {
            var missionApplication = _appDbContext.MissionApplications.Include(ma => ma.User).Include(ma => ma.Mission).ToList();
            if (searchText != "" && searchText != null)
            {
                return missionApplication.Where(m => m.Mission!.Title.ToLower().Contains(searchText.ToLower())).ToList();

            }
            return null!;

        }
    }
}

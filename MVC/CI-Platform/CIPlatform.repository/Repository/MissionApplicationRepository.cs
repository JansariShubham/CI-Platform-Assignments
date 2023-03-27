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

        public List<MissionApplication> getAllMissionApplication()
        {
            return _appDbContext.MissionApplications.Include(ma => ma.Mission).ToList();
        }
    }
}

using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class MissionInviteRepository : Repository<MissionInvite>, IMissionInviteRepository
    {
        private AppDbContext _appContext;
        public MissionInviteRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appContext = appDbContext;
        }
    }
}

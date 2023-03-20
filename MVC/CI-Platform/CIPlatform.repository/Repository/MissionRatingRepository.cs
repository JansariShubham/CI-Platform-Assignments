using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class MissionRatingRepository : Repository<MissionRating>, IMissionRatingRepository
    {
        AppDbContext _appDbContext;
        public MissionRatingRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext= appDbContext;
        }
    }
}

using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class CmsRepository : Repository<CmsPage>, ICmsRepository
    {
        private AppDbContext _appDbContext;
        public CmsRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}

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
    public class BannerRepository : Repository<Banner>, IBannerRepository
    {
        private AppDbContext _appDbContext;
        public BannerRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;   
        }

        public int ChangeStatus(int bannerId, bool status)
        {
            string query = "UPDATE banner set status = {0} WHERE banner_id = {1}";
            return _appDbContext.Database.ExecuteSqlRaw(query, status, bannerId);
        }

        public void Update(Banner banner)
        {
            _appDbContext.Banners.Update(banner);
        }
    }
}

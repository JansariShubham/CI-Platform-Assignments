using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        public int ChangeCmsStatus(int id, byte status)
        {
            var cmsID = new SqlParameter("@cms_page_id", id);
            var cmsStatus = new SqlParameter("@status", status);
            return _appDbContext.Database.ExecuteSqlRaw("UPDATE [cms_page] set status = @status WHERE cms_page_id = @cms_page_id", cmsID, cmsStatus);
        }

        public List<CmsPage> getSearchedCms(string? searchText)
        {
            if (searchText != "" && searchText != null)
            {
                return _appDbContext.CmsPages.Where(c => c.Title.ToLower().Contains(searchText.ToLower())).ToList();
            }
            return null;
        }

        public void Update(CmsPage obj)
        {
            _appDbContext.CmsPages.Update(obj);
        }
    }

}

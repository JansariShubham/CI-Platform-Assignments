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
    public class TimeSheetRepository : Repository<Timesheet>, ITimeSheetRepository
    {
        private AppDbContext _appDbContext;
        public TimeSheetRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public List<Timesheet> getTimeSheetList()
        {
           return  _appDbContext.Timesheets.Include(t => t.Mission).ToList();
        }
    }
}

using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class NotificationSettingRepository : Repository<NotificationSetting>, INotificationSettingRepository
    {
        private AppDbContext _appDbContext;
        public NotificationSettingRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;   
        }
    }
}

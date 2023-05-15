using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class UserNotificationRepository : Repository<UserNotification>, IUserNotificationRepository
    {
        private AppDbContext _appDbContext;
        public UserNotificationRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;   
        }
    }
}

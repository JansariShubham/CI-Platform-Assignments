using CIPlatform.entities.DataModels;
using CIPlatform.repository.Data;
using CIPlatform.repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public void save(User user)
        {
            Console.WriteLine(user + "Saved..");
            _appDbContext.SaveChanges();
        }

        public void Update(User user)
        {
            _appDbContext.Update(user);
        }
    }
}

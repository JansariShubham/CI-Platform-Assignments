using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;

namespace CIPlatform.repository.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
        

        public void Update(User user)
        {
            _appDbContext.Users.Update(user);
        }
    }
}

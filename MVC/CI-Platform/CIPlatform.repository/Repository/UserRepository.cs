using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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

        public void UpadateUserPassword(String Email, String Password)
        {
            var userEmail = new SqlParameter("@email", Email);
            var password = new SqlParameter("@password",Password);

            _appDbContext.Database.ExecuteSqlRaw("UPDATE [users] SET password = @password WHERE email = @email", userEmail, password);
            _appDbContext.SaveChanges();
        }
    }
}

using CIPlatform.entities.DataModels;
using CIPlatform.entities.ViewModels;
using CIPlatform.repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;



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

        public User GetLoginCredentials(UserLoginViewModel model)
        {
            Func<User, bool> value = (user) =>
            {
                return (user.Email == model.Email && user.Password.Equals(model.Password));
            };

             return _appDbContext.Users.FirstOrDefault(value);
        }

        public int UpadateUserPassword(String Email, String Password)
        {
            var userEmail = new SqlParameter("@email", Email);
            var password = new SqlParameter("@password",Password);

           return _appDbContext.Database.ExecuteSqlRaw("UPDATE [users] SET password = @password WHERE email = @email", userEmail, password);
            /*_appDbContext.SaveChanges();*/
        }


        public List<User> getAllUsers()
        {
           return _appDbContext.Users.Include(u => u.MissionInviteFromUsers)
                .Include(u => u.MissionInviteToUsers).ToList();
        }
    }
}

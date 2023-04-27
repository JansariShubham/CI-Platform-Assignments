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

        public Admin GetAdminLoginCredentials(UserLoginViewModel model)
        {
            Func<Admin, bool> value = (admin) =>
            {
                return (admin.Email == model.Email && admin.Password.Equals(model.Password));
            };

            return _appDbContext.Admins.FirstOrDefault(value);
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
                .Include(u => u.MissionInviteToUsers)
                .Include(u => u.StoryInviteToUsers)
                .Include(u => u.StoryInviteFromUsers)
                .Include(u=> u.UserSkills).ToList();
        }

        public int UpdateProfie(int? userId, string? url)
        {
            var userID = new SqlParameter("@user_id", userId);
            var avatar = new SqlParameter("@avatar", url);

            return _appDbContext.Database.ExecuteSqlRaw("UPDATE [users] set avatar = @avatar WHERE user_id = @user_id", userID, avatar);
        }

        public int ChangeUserStatus(long? userId, byte? status)
        {
            var userID = new SqlParameter("@user_id", userId);
            var userStatus = new SqlParameter("@status", status);

            return _appDbContext.Database.ExecuteSqlRaw("UPDATE [users] set status = @status WHERE user_id = @user_id", userID, userStatus);
        }

        public List<User> getSearchedResult(string? searchText)
        {
            //StringComparison comp = StringComparison.OrdinalIgnoreCase;
            
            if (searchText != "" && searchText != null)
            {
                var searchedResult = _appDbContext.Users.Where(u => u.FirstName.ToLower().Contains(searchText.ToLower()) || u.LastName.ToLower().Contains(searchText.ToLower()));
                return searchedResult.ToList();
            }
            return null;
        }
    }
}

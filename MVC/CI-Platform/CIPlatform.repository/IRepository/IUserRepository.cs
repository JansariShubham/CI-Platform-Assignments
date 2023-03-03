using CIPlatform.entities.DataModels;
using CIPlatform.entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.IRepository
{
    public interface IUserRepository: IRepository<User> 
    {
        void Update(User user);
        public int UpadateUserPassword(String email, String password);
        public User GetLoginCredentials(UserLoginViewModel model);
    }
}

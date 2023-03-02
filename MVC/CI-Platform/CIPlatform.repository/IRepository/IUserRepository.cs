using CIPlatform.entities.DataModels;
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
        public void UpadateUserPassword(String email, String password);
    }
}

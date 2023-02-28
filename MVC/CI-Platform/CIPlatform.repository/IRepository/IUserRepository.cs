using CIPlatform.entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.IRepository
{
    public interface IUserRepository: IRepository<User> 
    {
        void Update(User user);
        void save(User user);
    }
}

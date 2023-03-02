using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.IRepository
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IPasswordResetRepo PasswordResetRepo { get; }
        void Save();
    }
}

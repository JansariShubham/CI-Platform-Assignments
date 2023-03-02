using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository UserRepository { get; private set; }

        public  IPasswordResetRepo PasswordResetRepo { get; private set; }  

        private AppDbContext _appDbContext;

        public UnitOfWork(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
            UserRepository = new UserRepository(_appDbContext);
            PasswordResetRepo = new PasswordResetRepo(_appDbContext);
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}

using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public  class PasswordResetRepo : Repository<PasswordReset> , IPasswordResetRepo
    {
        private AppDbContext _appDbContext;

        public PasswordResetRepo(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public void Update(PasswordReset obj)
        {
            _appDbContext.PasswordResets.Update(obj);
        }
    }
}

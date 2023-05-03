using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private AppDbContext _appDbContext;
        public CommentRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public List<Comment> getAllComments()
        {
            var commentList = _appDbContext.Comments.Include(c => c.User).Include(c => c.Mission).OrderByDescending(c => c.CreatedAt);

            return commentList.ToList();
        }

    }
}

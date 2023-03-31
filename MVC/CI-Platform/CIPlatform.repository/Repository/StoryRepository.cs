using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.Repository
{
    public class StoryRepository : Repository<Story>, IStoryRepository
    {
        private AppDbContext _appDbContext;
        public StoryRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public List<Story> getAllStories()
        {
            return _appDbContext.Stories.Include(stories => stories.User)
                .Include(stories => stories.StoryMedia).ToList();
        }
        public Story GetStoryWithInclude(Expression<Func<Story, bool>> filter)
        {
            return dbSet.Include(stories => stories.StoryMedia).FirstOrDefault(filter)!;

        }

        public void Update(Story story)
        {
            _appDbContext.Update(story);
        }

        
        public void updateStoryViews(int? StoryId, long? StoryViews)
        {
            var storyId = new SqlParameter("@storyId", StoryId);
            var storyViews = new SqlParameter("@storyViews", StoryViews);

            _appDbContext.Database.ExecuteSqlRaw("UPDATE [story] SET story_views = @storyViews WHERE story_id = @storyId", storyId, storyViews);

        }

       
    }
}

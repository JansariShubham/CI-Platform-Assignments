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

        public int ApproveStoryStatus(int storyId)
        {
            string query = "UPDATE story SET status = {0} WHERE story_id = {1}";
            return _appDbContext.Database.ExecuteSqlRaw(query, 1, storyId);
        }

        public int DeclineStoryStatus(int storyId)
        {
            string query = "UPDATE story SET status = {0} WHERE story_id = {1}";
            return _appDbContext.Database.ExecuteSqlRaw(query, 0, storyId);
        }

        public List<Story> getAllStories()
        {
            return _appDbContext.Stories.Include(stories => stories.User)
                .Include(stories => stories.StoryMedia)
                .Include(stories => stories.Mission).ToList();
        }

        public List<Story> getSearchedStories(string? searchText)
        {
            var story = _appDbContext.Stories.Include(s => s.User).Include(s => s.Mission).ToList();
            if(searchText != null && searchText != "")
            {
                return story.Where(s => s.Title.ToLower().Contains(searchText.ToLower())).ToList();
            }
            return null!;
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

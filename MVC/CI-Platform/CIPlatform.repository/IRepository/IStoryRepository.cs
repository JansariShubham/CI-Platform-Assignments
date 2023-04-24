using CIPlatform.entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.repository.IRepository
{
    public interface IStoryRepository : IRepository<Story>
    {
        public List<Story> getAllStories();

        public Story GetStoryWithInclude(Expression<Func<Story, bool>> filter);

        void Update(Story story);

        void updateStoryViews(int? StoryId, long? StoryViews);


        public List<Story> getSearchedStories(string? searchText);

        public int ApproveStoryStatus(int storyId);

        public int DeclineStoryStatus(int storyId);


    }
}

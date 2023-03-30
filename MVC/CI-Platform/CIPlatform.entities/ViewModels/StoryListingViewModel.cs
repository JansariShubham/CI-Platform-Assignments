using CIPlatform.entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class StoryListingViewModel
    {
        public int StoryId { get; set; }

        public long? MissionId { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }

        public virtual User? User { get; set; }

        public string? imageUrl { get; set; }

        public long? StoryViews { get; set; }

        
    }
}

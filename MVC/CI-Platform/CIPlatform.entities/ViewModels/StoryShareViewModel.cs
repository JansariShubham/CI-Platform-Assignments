using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class StoryShareViewModel
    {
        public long? MissionId { get; set; }

        [Required(ErrorMessage = "Story title is required")]
        public string StoryTitle { get; set; } = string.Empty;            
        public string? Description { get; set; }

        public string? VideoUrl { get; set; }

        public string? MediaName { get; set; } 

        public string? MadiaPath { get; set; }

        public string? MediaType { get; set; }



    }
}

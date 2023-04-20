using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class CmsViewModel
    {

        public long? CmsPageId { get; set; }
        [Required(ErrorMessage = "Please Enter Title")]
        public string? Title { get; set; } = null!;
        [Required(ErrorMessage = "Please Enter Description")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Please Enter Slug")]
        public string? Slug { get; set; } = null!;
        [Required(ErrorMessage = "Please Select Status")]
        public bool Status { get; set; }

    }
}

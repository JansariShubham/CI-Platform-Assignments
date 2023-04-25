using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class BannerViewModel
    {
        public long? BannerId { get; set; }
        [Required(ErrorMessage = "Image is Required!")]
        public IFormFile? BannerImage { get; set; }
         public string? Path { get; set; }
        [Required(ErrorMessage = "Description is Required!")]
        public string? TextDesc { get; set; }
        [Required(ErrorMessage = "Title is Required!")]
        public string? TextTitle { get; set; }
        [Required(ErrorMessage = "Sort Order is Required!")]
        [RegularExpression("\\d+", ErrorMessage ="Please Enter only Numbers")]
        public int? SortOrder { get; set; }
        [Required(ErrorMessage = "Status is Required!")]
        public bool Status { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class ThemeViewModel
    {
        public long MissionThemeId { get; set; }

        [Required(ErrorMessage ="Please Enter Theme Title!")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = "Please Select Status!")]
        public byte Status { get; set; }
    }
}

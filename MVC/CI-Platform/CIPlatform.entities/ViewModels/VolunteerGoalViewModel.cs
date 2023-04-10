using CIPlatform.entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public  class VolunteerGoalViewModel
    {
        [Required(ErrorMessage ="Please select mission!")]
        public virtual Mission? Mission { get; set; }
        public List<PlatformLandingViewModel>? MissionList { get; set; }

        [Required(ErrorMessage = "Action is Required!")]

        public int? Action { get; set; }
        [Required(ErrorMessage = "Date is Required!")]
        public DateTimeOffset? Date { get; set; }

        [Required(ErrorMessage = "Message is Required!")]
        public string? Message { get; set; }

    }
}

using CIPlatform.entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class VolunteeringHoursViewModel
    {

        [Required(ErrorMessage = "Please select mission!")]
        public virtual Mission? Mission { get; set; }
        public List<PlatformLandingViewModel>? MissionList { get; set; }

        [Required(ErrorMessage = "Hour is Required!")]
        [RegularExpression("^(0?[0-9]|1[0-9]|2[0-3])$", ErrorMessage ="Hour must between 0 to 23")]
        public Double? Hour { get; set; }

        [Required(ErrorMessage = "Minutes is Required!")]
        [RegularExpression("^(0?[0-9]|[1-5][0-9])$", ErrorMessage ="Minutes must between 0 to 59")]
        public Double? Minutes { get; set; }

        [Required(ErrorMessage = "Date is Required!")]
        public DateTimeOffset? Date { get; set; }

        [Required(ErrorMessage = "Message is Required!")]
        public string? Message { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class ContactUsViewModel
    {
        public long? UserId { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }

        public string? email { get; set; }

        [Required(ErrorMessage = "Subject is Required!")]
        [MaxLength(255)]
        public string? Subject { get; set; } 

        [Required(ErrorMessage = "Message is Required!")]
        [MaxLength(6000)]
        public string? Message { get; set; }


    }
}

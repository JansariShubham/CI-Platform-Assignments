using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class SkillsViewModel
    {

        public int SkillId { get; set; }
        [Required(ErrorMessage ="Please Enter Skill Name!")]
        public string SkillName { get; set; } = null!;

        [Required(ErrorMessage = "Please Select Status!")]
        public bool Status { get; set; }
    }
}

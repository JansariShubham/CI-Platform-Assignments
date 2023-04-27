using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class MissionSkillViewModel
    {
        public int SkillId { get; set; }
        
        public string SkillName { get; set; } = null!;

    }
}

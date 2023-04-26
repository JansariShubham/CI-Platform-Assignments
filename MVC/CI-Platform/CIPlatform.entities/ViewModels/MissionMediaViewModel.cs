using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class MissionMediaViewModel
    {
        public long? MissionMediaId { get; set; }

        

        public string? MediaName { get; set; } 

        public string? MediaType { get; set; } 

        public string? MediaPath { get; set; }

        public bool? DefaultMedia { get; set; }

    }
}

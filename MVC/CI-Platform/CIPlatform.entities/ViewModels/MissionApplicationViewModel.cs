using CIPlatform.entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class MissionApplicationViewModel
    {
        public long? MissionApplicationId { get; set; }

        public long? UserId { get; set; }

        public long? MissionId { get; set; }

        public DateTimeOffset? AppliedAt { get; set; }

        public short? ApprovalStatus { get; set; }


        public virtual Mission? Mission { get; set; }

        public virtual User? User { get; set; }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class TimeSheetViewModel
    {
        public long? UserId { get; set; }

        public long? MissionId { get; set; }

        public DateTimeOffset? Time { get; set; }

        public int? Action { get; set; }

        public DateTimeOffset DateVolunteered { get; set; }

        public string? Message { get; set; }

        public bool? Status { get; set; }

        public List<PlatformLandingViewModel>? missionList { get; set; }


    }
}

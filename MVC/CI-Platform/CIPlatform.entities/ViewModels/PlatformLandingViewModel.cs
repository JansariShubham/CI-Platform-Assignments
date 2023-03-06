using CIPlatform.entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class PlatformLandingViewModel
    {
        public long MissionId { get; set; }

        public long? ThemeId { get; set; }

        public short? CityId { get; set; }

        public byte? CountryId { get; set; }

        public string Title { get; set; } = null!;

        public string ShortDesc { get; set; } = null!;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool MissionType { get; set; }

        public bool Status { get; set; }

        public string? OrgName { get; set; }

        public virtual City? City { get; set; }

        public virtual Country? Country { get; set; }
/*
        public virtual ICollection<FavouriteMission> FavouriteMissions { get; } = new List<FavouriteMission>();

        public virtual ICollection<GoalMission> GoalMissions { get; } = new List<GoalMission>();

        public virtual ICollection<MissionInvite> MissionInvites { get; } = new List<MissionInvite>();*/

        public virtual MissionTheme? Theme { get; set; }
    }
}

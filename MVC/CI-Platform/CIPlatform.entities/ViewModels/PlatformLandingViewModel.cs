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

        public string? OrgDetails { get; set; }

        public virtual City? City { get; set; }

        public virtual Country? Country { get; set; }

        public virtual FavouriteMission FavouriteMissions { get; }

        public virtual GoalMission GoalMissions { get; set; }

        public virtual MissionInvite MissionInvites { get; }
        public virtual MissionTheme? Theme { get; set; }

        public virtual String?  ThumbnailURL { get; set; }

        public virtual MissionSkill MissionSkills { get; set; }

        public int? TotalSeats { get; set; }

        public int? SeatsLeft { get; set; }
    }

}

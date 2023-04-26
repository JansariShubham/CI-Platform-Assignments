using CIPlatform.entities.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class GoalMissionViewModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }

        public string? OrgName { get; set; }
        public string? OrgDetail { get; set; }

        public long? MissionId { get; set; }

        public long? ThemeId { get; set; }

        public short? CityId { get; set; }

        public byte? CountryId { get; set; }

        public virtual City? City { get; set; }

        public virtual Country? Country { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public int? TotalSeats { get; set; }
        public byte? Availability { get; set; }
        public DateTimeOffset? RegDeadline { get; set; }

        public virtual MissionTheme? Theme { get; set; }
        public string? ShortDesc { get; set; }

        public virtual MissionSkill? MissionSkills { get; set; }
        public virtual ICollection<MissionMedia>? MissionMedia { get; set; }

        public virtual List<MissionDocumentVM>? MissionDocuments { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<GoalMission>? GoalMissions { get; set; }


        public virtual ICollection<SkillsViewModel>? SkillList { get; set; }
        public virtual ICollection<MissionMediaViewModel>? MissionMediaList { get; set; }

        public virtual List<MissionDocumentVM>? MissionDocumentsList { get; set; }
        public virtual ICollection<ThemeViewModel>? ThemeList { get; set; }


        public virtual ICollection<CityViewModel>? CityList { get; set; }

        public virtual ICollection<CountryViewModel>? CountryList { get; set; }
        public string? GoalObjectiveText { get; set; }

        public int? GoalValue { get; set; }


        public List<IFormFile>? Images { get; set; }

        public List<IFormFile>? Documents { get; set; }
    }
}

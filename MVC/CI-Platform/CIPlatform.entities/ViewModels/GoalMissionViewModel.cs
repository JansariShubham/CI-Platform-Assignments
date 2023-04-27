using CIPlatform.entities.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class GoalMissionViewModel
    {

        [Required(ErrorMessage = "Please Enter Title!")]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Please Enter Organization Name!")]
        public string? OrgName { get; set; }
        public string? OrgDetail { get; set; }

        public long? MissionId { get; set; }
        [Required(ErrorMessage = "Please Select Theme!")]
        public long? ThemeId { get; set; }
        [Required(ErrorMessage = "Please Select City!")]
        public short? CityId { get; set; }
        [Required(ErrorMessage = "Please Select Country!")]
        public byte? CountryId { get; set; }

        public virtual ICollection<CityViewModel>? CityList { get; set; }

        public virtual ICollection<CountryViewModel>? CountryList { get; set; }
        [Required(ErrorMessage = "Please Select Start Date!")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "Please Select End Date!")]
        public DateTime? EndDate { get; set; }
        [Required(ErrorMessage = "Please Enter Total Seats!")]
        [RegularExpression("\\d+", ErrorMessage = "Please Enter only Numbers")]
        public int? TotalSeats { get; set; }
        [Required(ErrorMessage = "Please Select Availability!")]
        public byte? Availability { get; set; }
        
        public DateTimeOffset? RegDeadline { get; set; }

        public virtual ICollection<ThemeViewModel>? ThemeList { get; set; }

        [Required(ErrorMessage = "Please Enter Short Description!")]
        public string? ShortDesc { get; set; }

        public virtual List<MissionSkillViewModel>? MissionSkills { get; set; }


        public virtual ICollection<SkillsViewModel>? SkillList { get; set; }
        public virtual ICollection<MissionMediaViewModel>? MissionMedia { get; set; }

        public virtual List<MissionDocumentVM>? MissionDocuments { get; set; }
        [Required(ErrorMessage = "Please Select Status!")]
        public bool? IsActive { get; set; }

        [Required(ErrorMessage = "Please Enter Goal Objective!")]
        public string? GoalObjectiveText { get; set; }

        [Required(ErrorMessage = "Please Enter Goal Value!")]
        [RegularExpression("\\d+", ErrorMessage = "Please Enter only Numbers")]
        public int? GoalValue { get; set; }


        public List<IFormFile>? Images { get; set; }

        public List<IFormFile>? Documents { get; set; }
    }
}

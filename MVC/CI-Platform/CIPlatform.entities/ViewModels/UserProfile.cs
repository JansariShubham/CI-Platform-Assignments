using CIPlatform.entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class UserProfile
    {
        [Required(ErrorMessage = "Enter Your Name")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Please Enter Only Characters")]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "First Name must be minimum 3 charcters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Enter Your Surname")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Please Enter Only Characters")]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "First Name must be minimum 3 charcters")]
        public string? LastName { get; set; }

        [StringLength(16)]
        public string? EmployeeId { get; set; }

        public string? Email { get; set; }

        [StringLength(255)]
        public string? Title { get; set; }

        [StringLength(16)]
        public string? Department { get; set; }

        [Required(ErrorMessage = "Enter Your Profile Summary")]
        public string? MyProfile { get; set; }
        public string? WhyIVolunteer { get; set; }


        public string? Avatar { get; set; }
        public short? CityId { get; set; }

        [Required(ErrorMessage = "Enter Your Country")]
        public byte? CountryId { get; set; }
        

        public string? LinkedinURL { get; set; }

        public virtual ICollection<Skill>? Skills { get; set; }


        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password must contain minimum 8 characters, 1 special character, number and capital letter")]
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and confirm password should match!")]
        public string? ConfirmPasword { get; set; }


        public string? OldPassword { get; set; }

        public List<SkillsViewModel>? SkillsList { get; set; }

        public List<CountryViewModel>? Countries { get; set; }

        public List<CityViewModel>? Cities { get; set; }

        


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CIPlatform.entities.ViewModels
{
    public class UserViewModel
    {

        public long UserId { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Please Enter Only Characters")]
        [StringLength(16,MinimumLength =3, ErrorMessage ="First Name must be minimum 3 charcters")]
        public string FirstName { get; set; } = null!;
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Please Enter Only Characters")]
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "Last Name must be minimum 3 charcters")]
        public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
+ "@"
+ @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "Invalid Email ID")]
        public string Email { get; set; } = null!;
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage ="Password must contain minimum 8 characters, 1 special character, number and capital letter")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and confirm password should match!")]
        public string ConfirmPasword { get; set; } = null!; 

        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(10, MinimumLength =10, ErrorMessage="Phone Number Must Contain atleast 10 Characters")]
        [RegularExpression("^[0-9]+$")]
        public String PhoneNumber { get; set; }

        
    }
}

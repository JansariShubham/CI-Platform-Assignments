using CIPlatform.entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class ResetPasswordViewModel : PasswordReset
    {
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password must contain minimum 8 characters, 1 special character, number and capital letter")]
        [Required(ErrorMessage = "Password is required")]
        public String Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and confirm password should match!")]
        public String ConfirmPassword { get; set; }
    }
}

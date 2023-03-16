using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
       /*[RegularExpression("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "Please Enter Valid Email")]*/
        public string Email { get; set; } = null!;
       /* [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password must contain minimum 8 characters, 1 special character, number and capital letter")]*/
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;

        public long UserId { get; set; }


    }
}

using LeaveManagementSystem.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LeaveManagementSystem.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "{0} is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "{0} should be in valid email address format")]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email already exist")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} should contain numbers only")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = " Password and Confirm Password does not match")]
        public string ConfirmPassword { get; set; }

        public RoleOptions Role { get; set; } = RoleOptions.Employee;
    }
}

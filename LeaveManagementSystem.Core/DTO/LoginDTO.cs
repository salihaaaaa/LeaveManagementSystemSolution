using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Core.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "{0} should be in valid email address format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}

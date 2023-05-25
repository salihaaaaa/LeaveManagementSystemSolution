using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Core.DTO
{
    public class LeaveAddRequest
    {
        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "Name")]
        public Guid? UserID { get; set; }

        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "Leave Type")]
        public Guid? LeaveTypeID { get; set; }

        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "{0} cannot be blank")]
        public string? Reason { get; set; }

        public StatusOptions? Status { get; set; } = StatusOptions.Pending;

        public Leave ToLeave()
        {
            return new Leave()
            {
                UserID = UserID,
                LeaveTypeID = LeaveTypeID,
                StartDate = StartDate,
                EndDate = EndDate,
                Reason = Reason,
                Status = Status.ToString()
            };
        }
    }
}

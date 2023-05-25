using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Core.DTO
{
    public class LeaveUpdateRequest
    {
        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "Leave ID")]
        public Guid LeaveID { get; set; }

        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "Name")]
        public Guid? UserID { get; set; }

        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "Leave Type")]
        public Guid? LeaveTypeID { get; set; }

        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "{0} cannot be blank")]
        public string? Reason { get; set; }

        public StatusOptions? Status { get; set; }

        public Leave ToLeave()
        {
            return new Leave()
            {
                LeaveID = LeaveID,
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

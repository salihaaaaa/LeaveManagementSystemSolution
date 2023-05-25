using LeaveManagementSystem.Core.Domain.IdentityEntities;
using LeaveManagementSystem.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Core.Domain.Entities
{
    public class Leave
    {
        [Key]
        public Guid LeaveID { get; set; }

        public Guid? UserID { get; set; }

        public Guid? LeaveTypeID { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(100)]
        public string? Reason { get; set; }

        [StringLength(10)]
        public string? Status { get; set; }

        public virtual LeaveType? LeaveType { get; set; }

        public virtual ApplicationUser? User { get; set; }
    }
}

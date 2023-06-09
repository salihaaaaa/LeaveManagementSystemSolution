using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Enums;

namespace LeaveManagementSystem.Core.DTO
{
    public class LeaveResponse
    {
        public Guid LeaveID { get; set; }
        public Guid? UserID { get; set; }
        public string? Name { get; set; }
        public Guid? LeaveTypeID { get; set; }
        public string? LeaveType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Reason { get; set; }
        public string? Status { get; set; }
        public double? Days { get; set; }

        public override string ToString()
        {
            return $"Leave ID: {LeaveID}, User ID: {UserID}, Name: {Name}, Leave Type ID: {LeaveTypeID}, Leave Type: {LeaveType}, Start Date: {StartDate}, End Date: {EndDate}, Reason: {Reason}, Status: {Status}";
        }

        public LeaveUpdateRequest ToLeaveUpdateRequest()
        {
            return new LeaveUpdateRequest()
            {
                LeaveID = LeaveID,
                UserID = UserID,
                LeaveTypeID = LeaveTypeID,
                StartDate = StartDate,
                EndDate = EndDate,
                Reason = Reason,
                Status = (StatusOptions)Enum.Parse(typeof(StatusOptions), Status, true)
            };
        }
    }

    public static class LeaveExtensions
    {
        public static LeaveResponse ToLeaveResponse(this Leave leave)
        {
            return new LeaveResponse()
            {
                LeaveID = leave.LeaveID,
                UserID = leave.UserID,
                Name = leave.User?.Name,
                LeaveTypeID = leave.LeaveTypeID,
                LeaveType = leave.LeaveType?.LeaveTypeName,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                Days = (leave.StartDate != null && leave.EndDate != null) ? ((leave.EndDate.Value - leave.StartDate.Value).Days) : null,
                Reason = leave.Reason,
                Status = leave.Status,
            };
        }
    }
}

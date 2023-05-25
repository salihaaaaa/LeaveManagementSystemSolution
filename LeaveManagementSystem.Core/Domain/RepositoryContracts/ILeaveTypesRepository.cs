using LeaveManagementSystem.Core.Domain.Entities;

namespace LeaveManagementSystem.Core.Domain.RepositoryContracts
{
    public interface ILeaveTypesRepository
    {
        Task<LeaveType> AddLeaveType(LeaveType leaveType);

        Task<List<LeaveType>> GetAlLeaveTypes();

        Task<LeaveType?> GetLeaveTypeByLeaveTypeID(Guid leaveTypeID);

        Task<LeaveType> UpdateLeaveType(LeaveType leaveType);

        Task<bool> DeleteLeaveType(Guid leaveTypeID);
    }
}

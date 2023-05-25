using LeaveManagementSystem.Core.DTO;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface ILeaveTypesGetterService
    {
        Task<List<LeaveTypeResponse>> GetAllLeaveTypes();

        Task<LeaveTypeResponse?> GetLeaveTypeByLeaveTypeID(Guid? leaveTypeID);
    }
}

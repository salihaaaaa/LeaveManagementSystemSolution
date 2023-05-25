using LeaveManagementSystem.Core.DTO;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface ILeaveTypesUpdaterService
    {
        Task<LeaveTypeResponse> UpdateLeaveType(LeaveTypeUpdateRequest? leaveTypeUpdateRequest);
    }
}

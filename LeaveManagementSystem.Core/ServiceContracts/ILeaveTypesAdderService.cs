using LeaveManagementSystem.Core.DTO;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface ILeaveTypesAdderService
    {
        Task<LeaveTypeResponse> AddLeaveType(LeaveTypeAddRequest? leaveTypeAddRequest);
    }
}

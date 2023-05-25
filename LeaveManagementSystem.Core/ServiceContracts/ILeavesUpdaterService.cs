using LeaveManagementSystem.Core.DTO;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface ILeavesUpdaterService
    {
        Task<LeaveResponse> UpdateLeave(LeaveUpdateRequest? leaveUpdateRequest);
    }
}

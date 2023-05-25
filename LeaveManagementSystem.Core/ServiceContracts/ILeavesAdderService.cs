using LeaveManagementSystem.Core.DTO;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface ILeavesAdderService
    {
        Task<LeaveResponse> AddLeave(LeaveAddRequest? leaveAddRequest);
    }
}

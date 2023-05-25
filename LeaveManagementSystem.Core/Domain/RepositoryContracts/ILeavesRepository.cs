using LeaveManagementSystem.Core.Domain.Entities;

namespace LeaveManagementSystem.Core.Domain.RepositoryContracts
{
    public interface ILeavesRepository
    {
        Task<Leave> AddLeave(Leave leave);

        Task<List<Leave>> GetAllLeaves();

        Task<Leave?> GetLeaveByLeaveID(Guid leaveID);

        Task<Leave> UpdateLeave(Leave leave);

        Task<bool> DeleteLeave(Guid leaveID);
    }
}

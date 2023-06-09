using LeaveManagementSystem.Core.Domain.Entities;

namespace LeaveManagementSystem.Core.Domain.RepositoryContracts
{
    public interface ILeaveRepository
    {
        Task<Leave> AddLeave(Leave leave);

        Task<List<Leave>> GetAllLeaves();

        Task<Leave?> GetLeaveByLeaveID(Guid leaveID);

        Task<List<Leave>> GetLeaveByUserID(Guid userID);

        Task<Leave> UpdateLeave(Leave leave);

        Task<bool> DeleteLeave(Guid leaveID);
    }
}

using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.ServiceContracts;

namespace LeaveManagementSystem.Core.Services
{
    public class LeavesDeleterService : ILeavesDeleterService
    {
        private readonly ILeavesRepository _leavesRepository;

        public LeavesDeleterService(ILeavesRepository leavesRepository)
        {
            _leavesRepository = leavesRepository;
        }

        public async Task<bool> DeleteLeave(Guid? leaveID)
        {
            if (leaveID == null)
            {
                throw new ArgumentNullException(nameof(leaveID));
            }

            Leave? leave = await _leavesRepository.GetLeaveByLeaveID(leaveID.Value);

            if (leave == null)
            {
                return false;
            }

            await _leavesRepository.DeleteLeave(leaveID.Value);
            return true;
        }
    }
}

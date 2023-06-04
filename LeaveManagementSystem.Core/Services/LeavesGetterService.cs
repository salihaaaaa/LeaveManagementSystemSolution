using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.ServiceContracts;

namespace LeaveManagementSystem.Core.Services
{
    public class LeavesGetterService : ILeavesGetterService
    {
        private readonly ILeavesRepository _leavesRepository;

        public LeavesGetterService(ILeavesRepository leavesRepository)
        {
            _leavesRepository = leavesRepository;
        }

        public async Task<List<LeaveResponse>> GetAllLeaves()
        {
            var leaves = await _leavesRepository.GetAllLeaves();

            return leaves
                .Select(temp => temp.ToLeaveResponse()).ToList();
        }

        public async Task<LeaveResponse?> GetLeaveByLeaveID(Guid? leaveID)
        {
            if (leaveID == null)
            {
                return null;
            }

            Leave? leave = await _leavesRepository.GetLeaveByLeaveID(leaveID.Value);

            if (leave == null)
            {
                return null;
            }

            return leave.ToLeaveResponse();
        }

        public async Task<List<LeaveResponse>> GetLeaveByUserID(Guid? userID)
        {
            var leaves = await _leavesRepository.GetLeaveByUserID(userID.Value);

            return leaves
                .Select(temp => temp.ToLeaveResponse()).ToList();
        }
    }
}

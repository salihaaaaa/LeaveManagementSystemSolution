using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.ServiceContracts;
using Services.Helpers;

namespace LeaveManagementSystem.Core.Services
{
    public class LeavesAdderService : ILeavesAdderService
    {
        private readonly ILeaveRepository _leavesRepository;

        public LeavesAdderService(ILeaveRepository leavesRepository)
        {
            _leavesRepository = leavesRepository;
        }

        public async Task<LeaveResponse> AddLeave(LeaveAddRequest? leaveAddRequest)
        {
            //Validation
            if (leaveAddRequest == null)
            {
                throw new ArgumentNullException(nameof(leaveAddRequest));
            }

            ValidationHelper.ModelValidation(leaveAddRequest);

            Leave leave = leaveAddRequest.ToLeave();
            leave.LeaveID = Guid.NewGuid();
            await _leavesRepository.AddLeave(leave);
            return leave.ToLeaveResponse();
        }
    }
}

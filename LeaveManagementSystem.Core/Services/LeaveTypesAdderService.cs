using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.ServiceContracts;
using Services.Helpers;

namespace LeaveManagementSystem.Core.Services
{
    public class LeaveTypesAdderService : ILeaveTypesAdderService
    {
        private readonly ILeaveTypesRepository _leaveTypesRepository;

        public LeaveTypesAdderService(ILeaveTypesRepository leaveTypesRepository)
        {
            _leaveTypesRepository = leaveTypesRepository;
        }

        public async Task<LeaveTypeResponse> AddLeaveType(LeaveTypeAddRequest? leaveTypeAddRequest)
        {
            //Validation
            if (leaveTypeAddRequest == null)
            {
                throw new ArgumentNullException(nameof(leaveTypeAddRequest));
            }

            ValidationHelper.ModelValidation(leaveTypeAddRequest);

            LeaveType leaveType = leaveTypeAddRequest.ToLeaveType();
            leaveType.LeaveTypeID = Guid.NewGuid();
            await _leaveTypesRepository.AddLeaveType(leaveType);
            return leaveType.ToLeaveTypeResponse();
        }
    }
}

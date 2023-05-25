using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.ServiceContracts;
using Services.Helpers;

namespace LeaveManagementSystem.Core.Services
{
    public class LeaveTypesUpdaterService : ILeaveTypesUpdaterService
    {
        private readonly ILeaveTypesRepository _leaveTypesRepository;

        public LeaveTypesUpdaterService(ILeaveTypesRepository leaveTypesRepository)
        {
            _leaveTypesRepository = leaveTypesRepository;
        }

        public async Task<LeaveTypeResponse> UpdateLeaveType(LeaveTypeUpdateRequest? leaveTypeUpdateRequest)
        {
            //Validation
            if (leaveTypeUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(leaveTypeUpdateRequest));
            }

            ValidationHelper.ModelValidation(leaveTypeUpdateRequest);

            //Get matching leave type object to update
            LeaveType? matchingLeaveType = await _leaveTypesRepository.GetLeaveTypeByLeaveTypeID(leaveTypeUpdateRequest.LeaveTypeID);

            if (matchingLeaveType == null)
            {
                throw new ArgumentException("Given leave type id doesn't exist");
            }

            //Update all details
            matchingLeaveType.LeaveTypeName = leaveTypeUpdateRequest.LeaveTypeName;

            await _leaveTypesRepository.UpdateLeaveType(matchingLeaveType);

            return matchingLeaveType.ToLeaveTypeResponse();
        }
    }
}

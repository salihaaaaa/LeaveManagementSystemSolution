using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.ServiceContracts;
using Services.Helpers;

namespace LeaveManagementSystem.Core.Services
{
    public class LeavesUpdaterService : ILeavesUpdaterService
    {
        private readonly ILeaveRepository _leavesRepository;

        public LeavesUpdaterService(ILeaveRepository leavesRepository)
        {
            _leavesRepository = leavesRepository;
        }

        public async Task<LeaveResponse> UpdateLeave(LeaveUpdateRequest? leaveUpdateRequest)
        {
            //Validation
            if (leaveUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(leaveUpdateRequest));
            }

            ValidationHelper.ModelValidation(leaveUpdateRequest);

            //Get matching leave object to update
            Leave? matchingLeave = await _leavesRepository.GetLeaveByLeaveID(leaveUpdateRequest.LeaveID);

            if (matchingLeave == null)
            {
                throw new ArgumentException("Given leave id doesn't exist");
            }

            //Update all details
            matchingLeave.LeaveTypeID = leaveUpdateRequest.LeaveTypeID;
            matchingLeave.StartDate = leaveUpdateRequest.StartDate;
            matchingLeave.EndDate = leaveUpdateRequest.EndDate;
            matchingLeave.Reason = leaveUpdateRequest.Reason;
            matchingLeave.Status = leaveUpdateRequest.Status.ToString();

            await _leavesRepository.UpdateLeave(matchingLeave);

            return matchingLeave.ToLeaveResponse();
        }
    }
}

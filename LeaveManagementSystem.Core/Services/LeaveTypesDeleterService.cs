using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.ServiceContracts;

namespace LeaveManagementSystem.Core.Services
{
    public class LeaveTypesDeleterService : ILeaveTypesDeleterService
    {
        private readonly ILeaveTypesRepository _leaveTypesRepository;

        public LeaveTypesDeleterService(ILeaveTypesRepository leaveTypesRepository)
        {
            _leaveTypesRepository = leaveTypesRepository;
        }

        public async Task<bool> DeleteLeaveType(Guid? leaveTypeID)
        {
            if (leaveTypeID == null)
            {
                throw new ArgumentNullException(nameof(leaveTypeID));
            }

            LeaveType? leaveType = await _leaveTypesRepository.GetLeaveTypeByLeaveTypeID(leaveTypeID.Value);

            if (leaveType == null)
            {
                return false;
            }

            await _leaveTypesRepository.DeleteLeaveType(leaveTypeID.Value);
            return true;
        }
    }
}

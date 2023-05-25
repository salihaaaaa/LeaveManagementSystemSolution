using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.ServiceContracts;
using System;

namespace LeaveManagementSystem.Core.Services
{
    public class LeaveTypesGetterService :  ILeaveTypesGetterService
    {
        private readonly ILeaveTypesRepository _leaveTypesRepository;

        public LeaveTypesGetterService(ILeaveTypesRepository leaveTypesRepository)
        {
            _leaveTypesRepository = leaveTypesRepository;
        }

        public async Task<List<LeaveTypeResponse>> GetAllLeaveTypes()
        {
            var leaveTypes = await _leaveTypesRepository.GetAlLeaveTypes();

            return leaveTypes
                .Select(temp => temp.ToLeaveTypeResponse()).ToList();
        }

        public async Task<LeaveTypeResponse?> GetLeaveTypeByLeaveTypeID(Guid? leaveTypeID)
        {
            if (leaveTypeID == null)
            {
                return null;
            }

            LeaveType? leaveType = await _leaveTypesRepository.GetLeaveTypeByLeaveTypeID(leaveTypeID.Value);

            if (leaveType == null)
            {
                return null;
            }

            return leaveType.ToLeaveTypeResponse();
        }
    }
}

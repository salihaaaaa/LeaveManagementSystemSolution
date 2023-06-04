using LeaveManagementSystem.Core.DTO;
using System;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface ILeavesGetterService
    {
        Task<List<LeaveResponse>> GetAllLeaves();

        Task<LeaveResponse?> GetLeaveByLeaveID(Guid? leaveID);

        Task<List<LeaveResponse>> GetLeaveByUserID(Guid? userID);
    }
}

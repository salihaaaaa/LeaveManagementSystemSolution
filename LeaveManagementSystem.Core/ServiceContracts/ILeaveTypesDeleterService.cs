using System;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface ILeaveTypesDeleterService
    {
        Task<bool>DeleteLeaveType(Guid? leaveTypeID);
    }
}

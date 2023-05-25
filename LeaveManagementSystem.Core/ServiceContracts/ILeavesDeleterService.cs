using System;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface ILeavesDeleterService
    {
        Task<bool> DeleteLeave(Guid? leaveID);
    }
}

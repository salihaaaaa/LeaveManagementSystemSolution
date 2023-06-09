using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.Enums;
using LeaveManagementSystem.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Infrastructure.Repositories
{
    public class LeavesRepository : ILeaveRepository
    {
        private readonly ApplicationDbContext _db;

        public LeavesRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Leave> AddLeave(Leave leave)
        {
            _db.Add(leave);
            await _db.SaveChangesAsync();
            return leave;
        }

        public async Task<bool> DeleteLeave(Guid leaveID)
        {
            _db.Leaves
                .RemoveRange(_db.Leaves
                .Where(temp => temp.LeaveID == leaveID));

            int rowsDeleted = await _db
                .SaveChangesAsync();

            return rowsDeleted > 0;
        }

        public async Task<List<Leave>> GetAllLeaves()
        {
            return await _db.Leaves
                .Include("User")
                .Include("LeaveType")
                .ToListAsync();
        }

        public async Task<Leave?> GetLeaveByLeaveID(Guid leaveID)
        {
            return await _db.Leaves
                .Include("User")
                .Include("LeaveType")
                .FirstOrDefaultAsync(temp => temp.LeaveID == leaveID);
        }

        public async Task<List<Leave>> GetLeaveByUserID(Guid userID)
        {
            return await _db.Leaves
                .Include("LeaveType")
                .Where(temp => temp.UserID == userID)
                .ToListAsync();
        }

        public async Task<Leave> UpdateLeave(Leave leave)
        {
            Leave matchingLeaves = await _db.Leaves.FirstAsync(temp => temp.LeaveID == leave.LeaveID);

            if (matchingLeaves == null)
            {
                return leave;
            }

            matchingLeaves.User = leave.User;
            matchingLeaves.LeaveTypeID = leave.LeaveTypeID;
            matchingLeaves.StartDate = leave.StartDate;
            matchingLeaves.EndDate = leave.EndDate;
            matchingLeaves.Reason = leave.Reason;
            matchingLeaves.Status = leave.Status;

            int countUpdated = await _db.SaveChangesAsync();
            return matchingLeaves;
        }
    }
}

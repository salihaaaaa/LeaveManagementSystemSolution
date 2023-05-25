using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System;

namespace LeaveManagementSystem.Infrastructure.Repositories
{
    public class LeaveTypesRepository : ILeaveTypesRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveTypesRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<LeaveType> AddLeaveType(LeaveType leaveType)
        {
            _db.Add(leaveType);
            await _db.SaveChangesAsync();
            return leaveType;
        }

        public async Task<bool> DeleteLeaveType(Guid leaveTypeID)
        {
            _db.LeaveTypes.RemoveRange(_db.LeaveTypes.Where(temp => temp.LeaveTypeID == leaveTypeID));

            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        }

        public async Task<List<LeaveType>> GetAlLeaveTypes()
        {
            return await _db.LeaveTypes.ToListAsync();
        }

        public async Task<LeaveType?> GetLeaveTypeByLeaveTypeID(Guid leaveTypeID)
        {
            return await _db.LeaveTypes.FirstOrDefaultAsync(temp => temp.LeaveTypeID == leaveTypeID);
        }

        public async Task<LeaveType> UpdateLeaveType(LeaveType leaveType)
        {
            LeaveType matchingLeaveTypes = await _db.LeaveTypes.FirstAsync(temp => temp.LeaveTypeID == leaveType.LeaveTypeID);

            if (matchingLeaveTypes == null)
            {
                return leaveType;
            }

            matchingLeaveTypes.LeaveTypeName = leaveType.LeaveTypeName;
            int countUpdated = await _db.SaveChangesAsync();
            return matchingLeaveTypes;
        }
    }
}

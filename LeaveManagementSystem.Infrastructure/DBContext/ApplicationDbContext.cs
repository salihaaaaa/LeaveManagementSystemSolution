using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Infrastructure.DBContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<Leave> Leaves { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<LeaveType>().ToTable("LeaveTypes");
            builder.Entity<Leave>().ToTable("Leaves");

            //Seed data to leaveTypes
            string leaveTypesJson = File.ReadAllText("leavetypes.json");
            List<LeaveType> leaveTypes = System.Text.Json.JsonSerializer.Deserialize<List<LeaveType>>(leaveTypesJson);

            foreach (LeaveType leaveType in leaveTypes)
            {
                builder.Entity<LeaveType>().HasData(leaveType);
            }
        }
    }
}

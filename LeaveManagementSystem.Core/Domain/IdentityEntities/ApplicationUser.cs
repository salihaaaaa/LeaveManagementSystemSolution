using LeaveManagementSystem.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LeaveManagementSystem.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? Name { get; set; }

        public virtual ICollection<Leave>? Leaves { get; set; }
    }
}

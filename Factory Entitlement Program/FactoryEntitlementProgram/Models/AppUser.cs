using Microsoft.AspNetCore.Identity;

namespace FactoryEntitlementProgram.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Role { get; set; } // "Admin" veya "User"

        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}

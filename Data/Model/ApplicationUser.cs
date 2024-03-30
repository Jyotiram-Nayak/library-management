using Microsoft.AspNetCore.Identity;

namespace library_management.Data.Model
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;

namespace Homefind.Infrastructure.Identity
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public int? UserType { get; set; }

        [PersonalData]
        public string DisplayName { get; set; }
    }
}

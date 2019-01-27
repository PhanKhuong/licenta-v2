using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homefind.Infrastructure.Identity
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserIdNumeric { get; set; }

        [PersonalData]
        public int? UserType { get; set; }

        [PersonalData]
        public string DisplayName { get; set; }
    }
}

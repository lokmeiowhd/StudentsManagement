using Microsoft.AspNetCore.Identity;

namespace MockSchoolManagement.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string City { get; set; }

    }
}

using Microsoft.AspNetCore.Identity;

namespace kurs.Models
{
    public class ApplicationUser: IdentityUser
    {
      public string? Gender { get; set; }
      public DateTime? DateOfEmployment { get; set; }

    }
}

using Microsoft.AspNetCore.Identity;

namespace Screening.WebAPI.Core.Data
{
    public class User : IdentityUser
    {
        public bool IsActiveDirectory { get; set; }
    }
}

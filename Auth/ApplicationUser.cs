using Microsoft.AspNetCore.Identity;

namespace TestWebAPIs.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}

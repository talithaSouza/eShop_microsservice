using Microsoft.AspNetCore.Identity;

namespace GeekShopping.IdentityServer.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
using Microsoft.AspNetCore.Identity;

namespace GeekShopping.IdentityServer.Entities
{
    public class ApplicationUser: IdentityUser
    {
        private string FirstName { get; set; }
        private string LastName { get; set; }
    }
}
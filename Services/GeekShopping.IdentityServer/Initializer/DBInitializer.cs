using System.Security.Claims;
using Duende.IdentityModel;
using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Entities;
using GeekShopping.IdentityServer.Entities.Context;
using Microsoft.AspNetCore.Identity;

namespace GeekShopping.IdentityServer.Initializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly MySqlContext _context;
        private UserManager<ApplicationUser> _users;
        private RoleManager<IdentityRole> _roles;

        public DBInitializer(MySqlContext context, UserManager<ApplicationUser> users, RoleManager<IdentityRole> roles)
        {
            _context = context;
            _users = users;
            _roles = roles;
        }


        public void Initializer()
        {
            if (_roles.FindByNameAsync(IdentityConfiguration.Admin).Result != null)
                return;

            _roles.CreateAsync(new IdentityRole(IdentityConfiguration.Admin)).GetAwaiter().GetResult();
            _roles.CreateAsync(new IdentityRole(IdentityConfiguration.Client)).GetAwaiter().GetResult();

            CreateAdminUser();
            CreateClientUser();
           
        }

        private void CreateAdminUser()
        {
            ApplicationUser admin = new()
            {
                UserName = "talitha-admin",
                Email = "talitha-admin@email.com.br",
                EmailConfirmed = true,
                PhoneNumber = "+55 (85) 12345-6789",
                FirstName = "Talitha",
                LastName = "Admin",
            };

            _users.CreateAsync(admin, "Talitha123$").GetAwaiter().GetResult();
            _users.AddToRoleAsync(admin, IdentityConfiguration.Admin).GetAwaiter().GetResult();

            var adminClaims = _users.AddClaimsAsync(admin, new Claim[]{
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin),
            }).Result;
        }

        private void CreateClientUser()
        {
             ApplicationUser client = new()
            {
                UserName = "talitha-client",
                Email = "talitha-client@email.com.br",
                EmailConfirmed = true,
                PhoneNumber = "+55 (85) 12345-6789",
                FirstName = "Talitha",
                LastName = "Client",
            };

            _users.CreateAsync(client, "Talitha123$").GetAwaiter().GetResult();
            _users.AddToRoleAsync(client, IdentityConfiguration.Client).GetAwaiter().GetResult();

            var clientClaims = _users.AddClaimsAsync(client, new Claim[]{
                new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                new Claim(JwtClaimTypes.GivenName, client.FirstName),
                new Claim(JwtClaimTypes.FamilyName, client.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client),
            }).Result;
        }
    }
}
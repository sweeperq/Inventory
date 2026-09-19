using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Inventory.Identity;

public class AppClaimsPrincipalFactory(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IOptions<IdentityOptions> options)
    : UserClaimsPrincipalFactory<AppUser, AppRole>(userManager, roleManager, options)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        if (!string.IsNullOrEmpty(user.FirstName))
            identity.AddClaim(new(ClaimTypes.GivenName, user.FirstName));
        if (!string.IsNullOrEmpty(user.LastName))
            identity.AddClaim(new(ClaimTypes.Surname, user.LastName));
        if (!string.IsNullOrEmpty(user.JobTitle))
            identity.AddClaim(new(AppClaimTypes.JobTitle, user.JobTitle));

        return identity;
    }
}

using Microsoft.AspNetCore.Identity;

namespace Inventory.Identity;

public class AppRole : IdentityRole<int>
{
    private AppRole() { }

    public AppRole(string roleName) : base(roleName)
    {
    }
}

using Microsoft.AspNetCore.Identity;

namespace WoV.Identity;

public class User : IdentityUser
{
    public Guid CharacterId { get; set; }
}
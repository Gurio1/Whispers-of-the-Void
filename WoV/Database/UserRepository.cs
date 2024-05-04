using Microsoft.EntityFrameworkCore;
using WoV.Identity;

namespace WoV.Database;

public class UserRepository(WoVDbContext context) : IUserRepository
{
    public Task SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}
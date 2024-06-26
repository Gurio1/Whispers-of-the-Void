using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WoV.CultivationStages;
using WoV.CultivationSubStages;
using WoV.Identity;

namespace WoV.Database;

public class WoVDbContext(DbContextOptions<WoVDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<User> ApplicationUsers { get; set; }
    public DbSet<CultivationStage> CultivationStages { get; set; }
    public DbSet<CultivationSubStage> CultivationSubStages { get; set; }
    public DbSet<Character> Characters { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("WoV");

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(builder);
    }
}
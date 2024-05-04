using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace WoV.Database;

public class CharacterRepository(WoVDbContext context) : ICharacterRepository
{
    public async Task<Character> CreateAsync(string userId)
    {
        var ch = new Character(userId);
        await context.Characters.AddAsync(ch);
        
        return ch;
    }

    public async Task<Character> GetByUserIdAsync(string userId)
    {
        return await context.Characters.FirstAsync(ch => ch.UserId == userId);
    }

    public async Task<List<Character>> GetByUserIdsAsync(List<string> userIds)
    {
        return await context.Characters.Where(ch => userIds.Contains(ch.UserId)).ToListAsync();
    }

    public Task SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}
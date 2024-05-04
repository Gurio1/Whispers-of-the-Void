using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace WoV;

public interface ICharacterRepository
{
    public Task<Character> CreateAsync(string userId);
    public Task<Character> GetByUserIdAsync(string userId);
    public Task<List<Character>> GetByUserIdsAsync(List<string> userIds);
    public Task SaveChangesAsync();
}
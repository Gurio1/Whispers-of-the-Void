using Microsoft.Extensions.Caching.Memory;
using ILogger = Serilog.ILogger;

namespace WoV.UserActivity;

public class UserActivityService(IMemoryCache cache,ILogger logger)
{
    private static List<string> _activeUserIds = [];
    public void UpdateUserActivity(string userId)
    {
        // Store user activity in cache with callback
        cache.Set(userId, true, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), // Example: expire after 10 minutes
            // Add callback when entry is removed from cache
            PostEvictionCallbacks = { new PostEvictionCallbackRegistration
            {
                EvictionCallback = (key, value, reason, state) =>
                {
                    logger.Information("User {UserId} was removed from cache. Reason: {Reason}", userId, reason);
                    _activeUserIds.Remove(userId);
                }
            }}
        });
        _activeUserIds.Add(userId);
        logger.Information("User {UserId} was added to the cache", userId);
    }

    public bool IsUserActive(string userId)
    {
        // Check if user is active in cache
        return cache.TryGetValue(userId, out _);
    }
    
    public void RemoveUserActivity(string userId)
    {
        cache.Remove(userId);
    }

    public List<string> GetAllActiveUserIds()
    {
        return _activeUserIds;
    }
}
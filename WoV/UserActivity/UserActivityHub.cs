using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WoV.UserActivity;

[Authorize]
public class UserActivityHub(UserActivityService userActivityService) : Hub
{
    public override async Task OnConnectedAsync()
    {
        Debug.Assert(Context.User != null, "Context.User != null");
        
        string userId = Context.User.FindFirstValue("ID") ?? throw new InvalidOperationException();
        userActivityService.UpdateUserActivity(userId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        Debug.Assert(Context.User != null, "Context.User != null");
        
        string userId = Context.User.FindFirstValue("ID") ?? throw new InvalidOperationException();
        userActivityService.RemoveUserActivity(userId);

        await base.OnDisconnectedAsync(exception);
    }
}
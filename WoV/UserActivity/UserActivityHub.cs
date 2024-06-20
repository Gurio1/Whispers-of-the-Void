using System.Diagnostics;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WoV.Cultivation.UseCases;
using WoV.UserActivity.UseCases;

namespace WoV.UserActivity;

[Authorize]
public class UserActivityHub(UserActivityService userActivityService, IMediator mediator) : Hub
{
    public override async Task OnConnectedAsync()
    {
        string userId = Context.User!.FindFirstValue("id") ?? throw new Exception("User dont have id claim");
        
        userActivityService.UpdateUserActivity(userId);

        await mediator.Send(new AddOfflineCultivationExpCommand(userId));
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string userId = Context.User!.FindFirstValue("id") ?? throw new Exception("User dont have id claim");
        userActivityService.RemoveUserActivity(userId);

        await mediator.Send(new AddLastTimeCharacterOnlineCommand(userId));
        
        await base.OnDisconnectedAsync(exception);
    }
}
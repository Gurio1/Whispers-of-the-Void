using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace WoV.SignalR;

public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User.FindFirstValue("id");
    }
}
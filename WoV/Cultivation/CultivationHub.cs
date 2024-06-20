using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WoV.Cultivation;

[Authorize]
public sealed class CultivationHub : Hub
{
    private readonly ICharacterRepository _characterRepository;

    public CultivationHub(ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }
    public override async Task OnConnectedAsync()
    {
        //TO DO:Create exception for this case
        var  userId = Context.User!.FindFirstValue("id") ?? throw new Exception("User dont have id claim");

        await Clients.Caller.SendAsync("U started cultivation!!");
        
        await base.OnConnectedAsync();
    }
}
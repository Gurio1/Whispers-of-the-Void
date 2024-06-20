using MediatR;
using Microsoft.AspNetCore.SignalR;
using WoV.UserActivity;

namespace WoV.Cultivation.UseCases;

public class AddCultivationExpHandler(
    ICharacterRepository characterRepository,
    IHubContext<UserActivityHub> hubContext)
    : IRequestHandler<AddCultivationExpCommand>
{
    public async Task Handle(AddCultivationExpCommand request, CancellationToken cancellationToken)
    {
        var ch = await characterRepository.GetByUserIdAsync(request.UserId);

        var cultivationExp = ch.Cultivate();

        await characterRepository.SaveChangesAsync();
        
        await hubContext.Clients.User(request.UserId).SendAsync("ReceiveNotification", cultivationExp, cancellationToken: cancellationToken);
    }
}
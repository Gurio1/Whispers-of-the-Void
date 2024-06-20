using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace WoV.Cultivation.UseCases;

public class AddOfflineCultivationExpHandler(
    ICharacterRepository characterRepository,
    IHubContext<CultivationHub> hubContext)
    : IRequestHandler<AddOfflineCultivationExpCommand>
{
    public async Task Handle(AddOfflineCultivationExpCommand request, CancellationToken cancellationToken)
    {
        var ch = await characterRepository.GetByUserIdAsync(request.UserId);

        var offlineCultivation = ch.CalculateOfflineCultivation();

        await characterRepository.SaveChangesAsync();
        
        await hubContext.Clients.User(request.UserId).SendAsync("ReceiveOfflineCultivationExp", offlineCultivation, cancellationToken: cancellationToken);
    }
}
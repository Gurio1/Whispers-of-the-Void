using MediatR;
using Microsoft.AspNetCore.SignalR;
using WoV.UserActivity;
using ILogger = Serilog.ILogger;

namespace WoV.Cultivation.UseCases;

public class AddBulkCultivationExpHandler(
    ICharacterRepository characterRepository,
    IHubContext<CultivationHub> hubContext,
    ILogger logger)
    : IRequestHandler<AddBulkCultivationExpCommand>
{
    public async Task Handle(AddBulkCultivationExpCommand request, CancellationToken cancellationToken)
    {
        var ch = await characterRepository.GetByUserIdsAsync(request.UserIds);

        foreach (var character in ch)
        {
            var cultivationExp = character.Cultivate();
            await hubContext.Clients.User(character.UserId).SendAsync("ReceiveNotification", cultivationExp, cancellationToken: cancellationToken);
        }
        logger.Information("Bulk cultivate completed");
        await characterRepository.SaveChangesAsync();
    }
}
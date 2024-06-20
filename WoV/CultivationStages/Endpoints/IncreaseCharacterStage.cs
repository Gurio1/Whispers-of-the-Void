using System.Security.Claims;
using FastEndpoints;
using MediatR;

namespace WoV.CultivationStages.Endpoints;

public class IncreaseCharacterStage(IMediator mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/cultivationStage/increase");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirstValue("id");

        var command = new IncreaseCharacterStageCommand(userId);

        await mediator.Send(command, ct);
        

        await SendAsync(ct, cancellation: ct);
    }
}
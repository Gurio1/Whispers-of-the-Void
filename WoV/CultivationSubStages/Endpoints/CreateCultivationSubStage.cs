using FastEndpoints;
using WoV.Database;

namespace WoV.CultivationSubStages.Endpoints;

public class CreateCultivationSubStage(WoVDbContext dbContext) : Endpoint<CreateCultivationSubStageRequest>
{
    public override void Configure()
    {
        Post("/cultivationSubStage");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CreateCultivationSubStageRequest req, CancellationToken ct)
    {
        var subStage = new CultivationSubStage()
        {
            CultivationStageId = req.CultivationStageId,
            Name = req.Name,
            Order = req.Order
        };

        dbContext.CultivationSubStages.Add(subStage);
        
        await dbContext.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }
}
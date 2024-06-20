using FastEndpoints;
using WoV.Database;

namespace WoV.CultivationStages.Endpoints;

public class CreateCultivationStage(WoVDbContext dbContext) : Endpoint<CreateCultivationStageRequest>
{
    public override void Configure()
    {
        Post("/cultivationStage");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CreateCultivationStageRequest req, CancellationToken ct)
    {
        var stage = new CultivationStage()
        {
            Name = req.Name,
            Order = req.Order
        };

        dbContext.CultivationStages.Add(stage);
        
        await dbContext.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }
}
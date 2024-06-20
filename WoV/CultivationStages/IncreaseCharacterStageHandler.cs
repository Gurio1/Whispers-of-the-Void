using MediatR;
using Microsoft.EntityFrameworkCore;
using WoV.Database;

namespace WoV.CultivationStages;

public class IncreaseCharacterStageHandler : IRequestHandler<IncreaseCharacterStageCommand>
{
    private readonly WoVDbContext _dbContext;

    public IncreaseCharacterStageHandler(WoVDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Handle(IncreaseCharacterStageCommand request, CancellationToken cancellationToken)
    {
        var ch = await _dbContext.Characters.Include(c => c.CultivationStage.SubStages)
            .Include(c => c.CultivationSubStage)
            .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken: cancellationToken);

        Console.ReadLine();


    }
    
    /*private void CheckForStageProgression()
    {
        // Check if the current sub-stage is completed
        if (!(CurrentCultivationExp >= SubCultivationStage.RequiredExperience)) return;
        
        var nextSubStageIndex = CultivationStage.SubStages.IndexOf(SubCultivationStage) + 1;

        if (nextSubStageIndex < CultivationStage.SubStages.Count)
        {
            // Move to the next sub-stage
            SubCultivationStage = CultivationStage.SubStages[nextSubStageIndex];
            CurrentCultivationExp -= SubCultivationStage.RequiredExperience;
        }
        else
        {
            // Move to the next main stage if available
            var nextStage = FindNextCultivationStage();
            if (nextStage != null)
            {
                CultivationStage = nextStage;
                SubCultivationStage = CultivationStage.SubStages.First();
                CurrentCultivationExp -= SubCultivationStage.RequiredExperience;
            }
        }
    }*/
}
using WoV.Combat;
using WoV.Cultivation;
using WoV.CultivationStages;
using WoV.CultivationSubStages;

namespace WoV;

public class Character(string userId) : ICombatable, ICultivable
{
    public string UserId { get; private set; } = userId;
    public DateTime LastTimeOnline { get; private set; }
    public Guid Id { get; private set; } = Guid.NewGuid();

    public double HP { get; private set; } = 100;
    public double Mana { get; private set; } = 100;
    public double Damage { get; private set; } = 10;
    public double Defence { get; private set; } = 40;

    public double TotalCultivationExp { get; private set;}
    public double CurrentCultivationExp { get; private set; }


    public int CultivationStageId { get; set; }
    public CultivationStage CultivationStage { get; private set; }
    
    public int CultivationSubStageId { get; set; }
    public CultivationSubStage CultivationSubStage { get; private set; }
    public double CultivationSpeedPerSecond { get; private set; } = 100.0;

    public bool IsCultivating { get; private set; }

    public void Attack(ICombatable opponent)
    {
        throw new NotImplementedException();
    }

    public double Cultivate()
    {
        if (!IsCultivating) return CurrentCultivationExp;
        
        TotalCultivationExp += CultivationSpeedPerSecond;
        CurrentCultivationExp += CultivationSpeedPerSecond;

        return CurrentCultivationExp;
    }

    public double CalculateOfflineCultivation()
    {
        if (!IsCultivating) return 0;
        
        var totalOfflineTime = DateTime.UtcNow - LastTimeOnline;

        var offlineCultivationExp = totalOfflineTime.TotalSeconds * CultivationSpeedPerSecond;
        TotalCultivationExp += offlineCultivationExp;
        CurrentCultivationExp += offlineCultivationExp;

        return offlineCultivationExp;
    }

    public void SetLastTimeOnline()
    {
        LastTimeOnline = DateTime.UtcNow;
    }
    
    private void CheckForStageProgression()
    {
        // Check if the current sub-stage is completed
        if (!(CurrentCultivationExp >= CultivationSubStage.RequiredExperience)) return;
        
        var nextSubStageIndex = CultivationStage.SubStages.IndexOf(CultivationSubStage) + 1;

        if (nextSubStageIndex < CultivationStage.SubStages.Count)
        {
            // Move to the next sub-stage
            CultivationSubStage = CultivationStage.SubStages[nextSubStageIndex];
            CurrentCultivationExp -= CultivationSubStage.RequiredExperience;
        }
        else
        {
            /*// Move to the next main stage if available
            var nextStage = FindNextCultivationStage();
            if (nextStage != null)
            {
                CultivationStage = nextStage;
                SubCultivationStage = CultivationStage.SubStages.First();
                CurrentCultivationExp -= SubCultivationStage.RequiredExperience;
            }*/
        }
    }
}
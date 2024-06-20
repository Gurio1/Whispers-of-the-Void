using WoV.Cultivation;
using WoV.CultivationSubStages;

namespace WoV.CultivationStages;

public class CultivationStage
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    public int Order { get; set; }
    
    public List<CultivationSubStage> SubStages { get; set; }
}
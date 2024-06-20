namespace WoV.CultivationSubStages;

public class CultivationSubStage
{
    public int Id { get; set; }
    public int CultivationStageId { get; set; }
    
    public string Name { get; set; }
    public int Order { get; set; }
    public double RequiredExperience { get; set; }
}
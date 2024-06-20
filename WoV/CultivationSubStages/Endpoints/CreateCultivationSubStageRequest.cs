namespace WoV.CultivationSubStages.Endpoints;

public record CreateCultivationSubStageRequest(string Name,int Order,int CultivationStageId);
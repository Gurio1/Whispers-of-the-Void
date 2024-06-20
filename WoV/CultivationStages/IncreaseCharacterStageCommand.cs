using MediatR;

namespace WoV.CultivationStages;

public record IncreaseCharacterStageCommand(string UserId) : IRequest;
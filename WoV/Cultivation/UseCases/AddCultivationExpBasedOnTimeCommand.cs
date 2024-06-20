using MediatR;

namespace WoV.Cultivation.UseCases;

public record AddCultivationExpBasedOnTimeCommand(Guid CharacterId,double Seconds) : IRequest;
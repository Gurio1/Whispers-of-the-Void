using MediatR;

namespace WoV.Cultivation.UseCases;

public record AddCultivationExpCommand(string UserId,double Exp) : IRequest;
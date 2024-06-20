using MediatR;

namespace WoV.Cultivation.UseCases;

public record AddOfflineCultivationExpCommand(string UserId) : IRequest;
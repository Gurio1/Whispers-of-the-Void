using MediatR;

namespace WoV.Cultivation.UseCases;

public record AddBulkCultivationExpCommand(List<string> UserIds) : IRequest;
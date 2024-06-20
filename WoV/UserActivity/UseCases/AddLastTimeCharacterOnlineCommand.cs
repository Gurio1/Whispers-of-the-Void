using MediatR;

namespace WoV.UserActivity.UseCases;

public record AddLastTimeCharacterOnlineCommand(string UserId) : IRequest;
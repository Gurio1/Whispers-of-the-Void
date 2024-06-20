using MediatR;

namespace WoV.UserActivity.UseCases;

public class AddLastTimeCharacterOnlineHandler(ICharacterRepository characterRepository)
    : IRequestHandler<AddLastTimeCharacterOnlineCommand>
{
    public async Task Handle(AddLastTimeCharacterOnlineCommand request, CancellationToken cancellationToken)
    {
        var ch = await characterRepository.GetByUserIdAsync(request.UserId);
        
        ch.SetLastTimeOnline();

        await characterRepository.SaveChangesAsync();
    }
}
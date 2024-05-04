using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace WoV.Identity.Endpoints;

public class CreateUser : Endpoint<CreateUserRequest>
{
    private readonly UserManager<User> _userManager;
    private readonly ICharacterRepository _characterRepository;

    public CreateUser(UserManager<User> userManager,ICharacterRepository characterRepository)
    {
        _userManager = userManager;
        _characterRepository = characterRepository;
    }

    public override void Configure()
    {
        Post("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        var newUser = new User()
        {
            Email = req.Email,
            UserName = req.Email,
        };
        
        var result = await _userManager.CreateAsync(newUser,req.Password);

        if (!result.Succeeded)
        {
            await SendAsync(result.Errors, 400, ct);
        }
        
        var ch = await _characterRepository.CreateAsync(newUser.Id);
        newUser.CharacterId = ch.Id;

        await _characterRepository.SaveChangesAsync();
        await SendOkAsync(ct);
    }
}
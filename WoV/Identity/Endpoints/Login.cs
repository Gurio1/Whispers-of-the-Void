using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;

namespace WoV.Identity.Endpoints;

public class Login : Endpoint<UserLoginRequest>
{
    private readonly UserManager<User> _userManager;

    public Login(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public override void Configure()
    {
        Post("users/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UserLoginRequest req, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(req.Email);

        if (user == null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var loginSuccessful = await _userManager.CheckPasswordAsync(user, req.Password);

        if (!loginSuccessful)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var jwtSecret = Config["Auth:JwtSecret"]!;
        var jwtToken = JwtBearer.CreateToken(
            o =>
            {
                o.SigningKey = jwtSecret;
                o.ExpireAt = DateTime.UtcNow.AddDays(1);
                o.User["ID"] = user.Id;
            });

        await SendOkAsync(jwtToken, ct);

    }
}
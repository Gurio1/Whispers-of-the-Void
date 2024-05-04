using System.Text.RegularExpressions;
using FastEndpoints;
using FluentValidation;

namespace WoV.Identity.Endpoints;

public class CreateUserRequestValidator : Validator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email address is required")
            .EmailAddress()
            .WithMessage("Invalid email");


        RuleFor(x => x.Password)
            .MinimumLength(8)
            .WithMessage("Password cant be less than 8 symbols")
            .NotEmpty()
            .WithMessage("Password required")
            .Equal(x => x.ConfirmedPassword)
            .WithMessage("Password and Confirmed password should be equal")
            .Must(BeValidPassword)
            .WithMessage(
                "Password should contain at least one uppercase, one lowercase letter and at least one special symbol");
    }

    private bool BeValidPassword(string password)
    {
        // Check for at least one uppercase and one lowercase letter
        if (!password.Any(char.IsUpper) || !password.Any(char.IsLower))
        {
            return false;
        }

        // Check for at least one special symbol
        if (!Regex.IsMatch(password, @"[!@#$%^&*()_+{}\[\]:;<>,.?/~\-]"))
        {
            return false;
        }

        return true;
    }
}
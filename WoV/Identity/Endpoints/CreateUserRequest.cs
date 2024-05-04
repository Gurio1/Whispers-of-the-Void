namespace WoV.Identity.Endpoints;

public record CreateUserRequest(string Email,string Password,string ConfirmedPassword);
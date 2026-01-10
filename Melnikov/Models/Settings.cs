namespace Melnikov.Models;

public class SignInSettings
{
    public string LoginOrEmail { get; set; } = string.Empty;
}

public sealed class AuthenticationSettings
{
    public string Token { get; set; } = string.Empty;
}

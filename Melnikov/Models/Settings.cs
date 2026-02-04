using Gaia.Services;

namespace Melnikov.Models;

public class SignInSettings : ObjectStorageValue<SignInSettings>
{
    public string LoginOrEmail { get; set; } = string.Empty;
}

public sealed class AuthenticationSettings : ObjectStorageValue<AuthenticationSettings>
{
    public string Token { get; set; } = string.Empty;
}

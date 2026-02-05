using Gaia.Services;

namespace Melnikov.Models;

public sealed class AuthenticationSettings : ObjectStorageValue<AuthenticationSettings>
{
    public string Token { get; set; } = string.Empty;
    public string LoginOrEmail { get; set; } = string.Empty;
    public bool IsRememberMe { get; set; }
}

using System.Text.Json;
using Jab;
using Manis.Contract.Services;
using Melnikov.Models;
using ManisJsonContext = Manis.Contract.Models.ManisJsonContext;

namespace Melnikov.Services;

[ServiceProviderModule]
[Transient(typeof(IAuthenticationService), Factory = nameof(GetAuthenticationService))]
public interface IMelnikovServiceProvider
{
    public static IAuthenticationService GetAuthenticationService(AuthenticationServiceOptions options)
    {
        return new AuthenticationService(new()
        {
            BaseAddress = new(options.Url),
        }, new()
        {
            TypeInfoResolver = ManisJsonContext.Resolver,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
    }
}
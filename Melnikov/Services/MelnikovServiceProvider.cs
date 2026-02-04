using System.Text.Json;
using Gaia.Helpers;
using Gaia.Services;
using Jab;
using Manis.Contract.Services;
using Melnikov.Models;
using ManisJsonContext = Manis.Contract.Models.ManisJsonContext;

namespace Melnikov.Services;

[ServiceProviderModule]
[Transient(typeof(IAuthenticationService), Factory = nameof(GetAuthenticationService))]
[Singleton(typeof(IAuthenticationUiService), typeof(AuthenticationUiService))]
[Transient(typeof(IMelnikovViewModelFactory), typeof(MelnikovViewModelFactory))]
public interface IMelnikovServiceProvider
{
    public static IAuthenticationService GetAuthenticationService(
        AuthenticationServiceOptions options,
        HttpClient httpClient
    )
    {
        httpClient.BaseAddress = new(options.Url);

        return new AuthenticationHttpService(
            httpClient,
            new()
            {
                TypeInfoResolver = ManisJsonContext.Resolver,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            },
            new TryPolicyService(3, TimeSpan.FromSeconds(1), FuncHelper<Exception>.Empty),
            EmptyHeadersFactory.Instance
        );
    }
}

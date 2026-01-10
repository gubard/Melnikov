using System.Text.Json;
using Gaia.Services;
using Inanna.Models;
using Jab;
using Manis.Contract.Services;
using Melnikov.Models;
using ManisJsonContext = Manis.Contract.Models.ManisJsonContext;

namespace Melnikov.Services;

[ServiceProviderModule]
[Transient(typeof(IAuthenticationService), Factory = nameof(GetAuthenticationService))]
[Singleton(typeof(IUiAuthenticationService), typeof(UiAuthenticationService))]
public interface IMelnikovServiceProvider
{
    public static IAuthenticationService GetAuthenticationService(
        AuthenticationServiceOptions options,
        HttpClient httpClient,
        AppState appState
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
            new TryPolicyService(
                3,
                TimeSpan.FromSeconds(1),
                _ => appState.SetServiceMode(nameof(AuthenticationHttpService), ServiceMode.Offline)
            ),
            EmptyHeadersFactory.Instance
        );
    }
}

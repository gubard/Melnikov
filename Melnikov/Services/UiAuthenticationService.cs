using System.Runtime.CompilerServices;
using Gaia.Models;
using Inanna.Services;
using Manis.Contract.Models;
using Manis.Contract.Services;
using Melnikov.Models;

namespace Melnikov.Services;

public interface IUiAuthenticationService : IAuthenticationService
{
    TokenResult? Token { get; }
    event Action<TokenResult> LoggedIn;
    event Action LoggedOut;
    void Logout();
    void Login(string token);
}

public class UiAuthenticationService : IUiAuthenticationService
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ISettingsService<MelnikovSettings> _settingsService;

    public UiAuthenticationService(
        IAuthenticationService authenticationService,
        ISettingsService<MelnikovSettings> settingsService
    )
    {
        _authenticationService = authenticationService;
        _settingsService = settingsService;
    }

    public TokenResult? Token { get; private set; }
    public event Action<TokenResult>? LoggedIn;
    public event Action? LoggedOut;

    public void Logout()
    {
        Token = null;
        _settingsService.SaveSettings(new() { Token = string.Empty, Id = Guid.Empty });
        LoggedOut?.Invoke();
    }

    public void Login(string token)
    {
        Token = new() { Token = token };
        LoggedIn?.Invoke(Token);
    }

    public ConfiguredValueTaskAwaitable<ManisGetResponse> GetAsync(
        ManisGetRequest request,
        CancellationToken ct
    )
    {
        return GetCore(request, ct).ConfigureAwait(false);
    }

    private async ValueTask<ManisGetResponse> GetCore(ManisGetRequest request, CancellationToken ct)
    {
        var response = await _authenticationService.GetAsync(request, ct);

        if (response.SignIns.Count == 0)
        {
            return response;
        }

        Token = response.SignIns.First().Value;
        LoggedIn?.Invoke(Token);

        return response;
    }

    public ConfiguredValueTaskAwaitable<ManisPostResponse> PostAsync(
        ManisPostRequest request,
        CancellationToken ct
    )
    {
        return _authenticationService.PostAsync(request, ct);
    }

    public ManisPostResponse Post(ManisPostRequest request)
    {
        return _authenticationService.Post(request);
    }

    public ManisGetResponse Get(ManisGetRequest request)
    {
        var response = _authenticationService.Get(request);

        if (response.SignIns.Count == 0)
        {
            return response;
        }

        Token = response.SignIns.First().Value;
        LoggedIn?.Invoke(Token);

        return response;
    }
}

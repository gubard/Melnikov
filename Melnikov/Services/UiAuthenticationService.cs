using Gaia.Models;
using Manis.Contract.Models;
using Manis.Contract.Services;

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

    public UiAuthenticationService(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public TokenResult? Token { get; private set; }
    public event Action<TokenResult>? LoggedIn;
    public event Action? LoggedOut;

    public void Logout()
    {
        Token = null;
        LoggedOut?.Invoke();
    }

    public void Login(string token)
    {
        Token = new() { Token = token };
        LoggedIn?.Invoke(Token);
    }

    public async ValueTask<ManisGetResponse> GetAsync(ManisGetRequest request, CancellationToken ct)
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

    public ValueTask<ManisPostResponse> PostAsync(ManisPostRequest request, CancellationToken ct)
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

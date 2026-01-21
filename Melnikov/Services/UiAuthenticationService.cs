using System.Runtime.CompilerServices;
using Gaia.Models;
using Gaia.Services;
using Manis.Contract.Models;
using Manis.Contract.Services;
using Melnikov.Models;

namespace Melnikov.Services;

public interface IUiAuthenticationService
{
    TokenResult? Token { get; }
    event Action<TokenResult> LoggedIn;
    event Action LoggedOut;
    ConfiguredValueTaskAwaitable LogoutAsync(CancellationToken ct);
    void Login(string token);

    ConfiguredValueTaskAwaitable<ManisGetResponse> GetAsync(
        ManisGetRequest request,
        bool isSaveToken,
        CancellationToken ct
    );

    ConfiguredValueTaskAwaitable<ManisPostResponse> PostAsync(
        Guid idempotentId,
        ManisPostRequest request,
        CancellationToken ct
    );
}

public class UiAuthenticationService : IUiAuthenticationService
{
    public UiAuthenticationService(
        IAuthenticationService authenticationService,
        IObjectStorage objectStorage
    )
    {
        _authenticationService = authenticationService;
        _objectStorage = objectStorage;
    }

    public TokenResult? Token { get; private set; }
    public event Action<TokenResult>? LoggedIn;
    public event Action? LoggedOut;

    public ConfiguredValueTaskAwaitable LogoutAsync(CancellationToken ct)
    {
        return LogoutCore(ct).ConfigureAwait(false);
    }

    public void Login(string token)
    {
        Token = new() { Token = token };
        LoggedIn?.Invoke(Token);
    }

    public ConfiguredValueTaskAwaitable<ManisGetResponse> GetAsync(
        ManisGetRequest request,
        bool isSaveToken,
        CancellationToken ct
    )
    {
        return GetCore(request, isSaveToken, ct).ConfigureAwait(false);
    }

    public ConfiguredValueTaskAwaitable<ManisPostResponse> PostAsync(
        Guid idempotentId,
        ManisPostRequest request,
        CancellationToken ct
    )
    {
        return _authenticationService.PostAsync(idempotentId, request, ct);
    }

    private readonly IAuthenticationService _authenticationService;
    private readonly IObjectStorage _objectStorage;

    private async ValueTask LogoutCore(CancellationToken ct)
    {
        Token = null;

        await _objectStorage.SaveAsync(
            $"{typeof(AuthenticationSettings).FullName}",
            new AuthenticationSettings { Token = string.Empty },
            ct
        );

        LoggedOut?.Invoke();
    }

    private async ValueTask<ManisGetResponse> GetCore(
        ManisGetRequest request,
        bool isSaveToken,
        CancellationToken ct
    )
    {
        var response = await _authenticationService.GetAsync(request, ct);

        if (response.SignIns.Count == 0)
        {
            return response;
        }

        Token = response.SignIns.First().Value;

        if (isSaveToken)
        {
            await _objectStorage.SaveAsync(
                $"{typeof(AuthenticationSettings).FullName}",
                new AuthenticationSettings
                {
                    Token = response.SignIns[request.SignIns.Keys.First()].Token,
                },
                ct
            );
        }
        else
        {
            await _objectStorage.SaveAsync(
                $"{typeof(AuthenticationSettings).FullName}",
                new AuthenticationSettings { Token = string.Empty },
                ct
            );
        }

        LoggedIn?.Invoke(Token);

        return response;
    }
}

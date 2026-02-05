using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using Avalonia.Threading;
using Gaia.Helpers;
using Gaia.Models;
using Gaia.Services;
using Inanna.Models;
using Manis.Contract.Models;
using Manis.Contract.Services;
using Melnikov.Models;

namespace Melnikov.Services;

public interface IAuthenticationUiService
{
    TokenResult? Token { get; }
    ConfiguredValueTaskAwaitable LogoutAsync(CancellationToken ct);
    ConfiguredValueTaskAwaitable LoginAsync(string token, CancellationToken ct);

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

public class AuthenticationUiService : IAuthenticationUiService
{
    public AuthenticationUiService(
        IAuthenticationService authenticationService,
        IObjectStorage objectStorage,
        AppState appState,
        JwtSecurityTokenHandler jwtSecurityTokenHandler
    )
    {
        _authenticationService = authenticationService;
        _objectStorage = objectStorage;
        _appState = appState;
        _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
    }

    public TokenResult? Token { get; private set; }

    public ConfiguredValueTaskAwaitable LogoutAsync(CancellationToken ct)
    {
        return LogoutCore(ct).ConfigureAwait(false);
    }

    public ConfiguredValueTaskAwaitable LoginAsync(string token, CancellationToken ct)
    {
        Token = new() { Token = token };
        UpdateUser();
        ct.ThrowIfCancellationRequested();

        return TaskHelper.ConfiguredCompletedTask;
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
    private readonly AppState _appState;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    private async ValueTask LogoutCore(CancellationToken ct)
    {
        Token = null;
        UpdateUser();
        var settings = await _objectStorage.LoadAsync<AuthenticationSettings>(ct);
        settings.Token = string.Empty;
        await _objectStorage.SaveAsync(settings, ct);
    }

    private async ValueTask<ManisGetResponse> GetCore(
        ManisGetRequest request,
        bool isRememberMe,
        CancellationToken ct
    )
    {
        var response = await _authenticationService.GetAsync(request, ct);

        if (response.ValidationErrors.Count != 0)
        {
            return response;
        }

        var signIn = response.SignIns.First();
        Token = signIn.Value;

        if (isRememberMe)
        {
            await _objectStorage.SaveAsync(
                new AuthenticationSettings
                {
                    Token = Token.Token,
                    LoginOrEmail = signIn.Key,
                    IsRememberMe = isRememberMe,
                },
                ct
            );
        }
        else
        {
            await _objectStorage.SaveAsync(
                new AuthenticationSettings
                {
                    Token = string.Empty,
                    LoginOrEmail = signIn.Key,
                    IsRememberMe = isRememberMe,
                },
                ct
            );
        }

        UpdateUser();

        return response;
    }

    private void UpdateUser()
    {
        if (Token == null)
        {
            Dispatcher.UIThread.Invoke(() => _appState.User = null);
        }
        else
        {
            if (!_jwtSecurityTokenHandler.CanReadToken(Token.Token))
            {
                throw new("Invalid token");
            }

            var jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(Token.Token);

            Dispatcher.UIThread.Invoke(() =>
                _appState.User = new()
                {
                    Id = Guid.Parse(jwtSecurityToken.Claims.GetNameIdentifierClaim().Value),
                    Login = jwtSecurityToken.Claims.GetNameClaim().Value,
                    Email = jwtSecurityToken.Claims.GetEmailClaim().Value,
                }
            );
        }
    }
}

using System.Runtime.CompilerServices;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gaia.Helpers;
using Gaia.Services;
using Inanna.Helpers;
using Inanna.Models;
using Inanna.Services;
using Manis.Contract.Models;
using Melnikov.Models;
using Melnikov.Services;

namespace Melnikov.Ui;

public partial class SignInViewModel : ViewModelBase, INonHeader, INonNavigate, IInitUi, ISaveUi
{
    public SignInViewModel(
        IAuthenticationUiService authenticationUiService,
        Func<CancellationToken, ConfiguredValueTaskAwaitable> successSignInFunc,
        IObjectStorage objectStorage,
        AppState appState
    )
    {
        _authenticationUiService = authenticationUiService;
        _successSignInFunc = successSignInFunc;
        _objectStorage = objectStorage;
        _appState = appState;
    }

    public ConfiguredValueTaskAwaitable InitUiAsync(CancellationToken ct)
    {
        return WrapCommandAsync(() => InitUiCore(ct).ConfigureAwait(false), ct);
    }

    public ConfiguredValueTaskAwaitable SaveUiAsync(CancellationToken ct)
    {
        return _objectStorage.SaveAsync(
            $"{typeof(SignInViewModel).FullName}",
            new SignInSettings { LoginOrEmail = LoginOrEmail },
            ct
        );
    }

    [ObservableProperty]
    private string _loginOrEmail = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _isRememberMe;

    private readonly IAuthenticationUiService _authenticationUiService;
    private readonly IObjectStorage _objectStorage;
    private readonly Func<CancellationToken, ConfiguredValueTaskAwaitable> _successSignInFunc;
    private readonly AppState _appState;

    private async ValueTask InitUiCore(CancellationToken ct)
    {
        var signInSettings = await _objectStorage.LoadAsync<SignInSettings>(
            $"{typeof(SignInViewModel).FullName}",
            ct
        );

        Dispatcher.UIThread.Post(() => LoginOrEmail = signInSettings.LoginOrEmail);

        var authenticationSettings = await _objectStorage.LoadAsync<AuthenticationSettings>(
            $"{typeof(AuthenticationSettings).FullName}",
            ct
        );

        if (!authenticationSettings.Token.IsNullOrWhiteSpace())
        {
            _authenticationUiService.Login(authenticationSettings.Token);
            await _successSignInFunc.Invoke(ct);
        }
    }

    [RelayCommand]
    private async Task SignInAsync(CancellationToken ct)
    {
        await WrapCommandAsync(() => SignInCore(ct).ConfigureAwait(false), ct);
    }

    private async ValueTask SignInCore(CancellationToken ct)
    {
        var response = await _authenticationUiService.GetAsync(
            CreateManisGetRequest(),
            IsRememberMe,
            ct
        );

        if (await UiHelper.CheckValidationErrorsAsync(response, ct))
        {
            _appState.ResetServiceModes();
            await _successSignInFunc.Invoke(ct);
        }
    }

    private ManisGetRequest CreateManisGetRequest()
    {
        return new() { SignIns = new() { { LoginOrEmail, Password } } };
    }
}

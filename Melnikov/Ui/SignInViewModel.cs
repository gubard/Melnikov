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

public partial class SignInViewModel
    : ViewModelBase,
        INonHeader,
        INonNavigate,
        IInitUi,
        INonStatusBar,
        ILoadUi
{
    public SignInViewModel(
        IAuthenticationUiService authenticationUiService,
        Func<CancellationToken, ConfiguredValueTaskAwaitable> successSignInFunc,
        IObjectStorage objectStorage,
        AppState appState,
        IServiceController serviceController
    )
    {
        _authenticationUiService = authenticationUiService;
        _successSignInFunc = successSignInFunc;
        _objectStorage = objectStorage;
        _appState = appState;
        _serviceController = serviceController;
    }

    public ConfiguredValueTaskAwaitable InitUiAsync(CancellationToken ct)
    {
        return WrapCommandAsync(
            async () =>
            {
                var settings = await _objectStorage.LoadAsync<AuthenticationSettings>(ct);

                Dispatcher.UIThread.Invoke(() =>
                {
                    LoginOrEmail = settings.LoginOrEmail;
                    IsRememberMe = settings.IsRememberMe;
                });

                if (!settings.Token.IsNullOrWhiteSpace())
                {
                    await _authenticationUiService.LoginAsync(settings.Token, ct);
                }
            },
            ct
        );
    }

    public ConfiguredValueTaskAwaitable LoadUiAsync(CancellationToken ct)
    {
        return WrapCommandAsync(
            async () =>
            {
                if (_appState.User is null)
                {
                    return new DefaultValidationErrors();
                }

                var errors = await _serviceController.RefreshServicesAsync(ct);
                await _successSignInFunc.Invoke(ct);

                return errors;
            },
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
    private readonly IServiceController _serviceController;

    [RelayCommand]
    private async Task SignInAsync(CancellationToken ct)
    {
        await WrapCommandAsync(() => SignInCore(ct).ConfigureAwait(false), ct);
    }

    private async ValueTask<IValidationErrors> SignInCore(CancellationToken ct)
    {
        var response = await _authenticationUiService.GetAsync(
            CreateManisGetRequest(),
            IsRememberMe,
            ct
        );

        if (response.ValidationErrors.Count != 0)
        {
            return response;
        }

        _appState.ResetServiceModes();
        var errors = await _serviceController.RefreshServicesAsync(ct);
        await _successSignInFunc.Invoke(ct);

        return errors;
    }

    private ManisGetRequest CreateManisGetRequest()
    {
        return new() { SignIns = new() { { LoginOrEmail.Trim(), Password } } };
    }
}

using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gaia.Helpers;
using Inanna.Helpers;
using Inanna.Models;
using Inanna.Services;
using Manis.Contract.Models;
using Melnikov.Models;
using Melnikov.Services;

namespace Melnikov.Ui;

public partial class SignInViewModel : ViewModelBase, INonHeader, INonNavigate
{
    [ObservableProperty]
    private string _loginOrEmail = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _isRememberMe;

    [ObservableProperty]
    private bool _isAvailableOffline;

    private readonly IUiAuthenticationService _uiAuthenticationService;
    private readonly ISettingsService<MelnikovSettings> _settingsService;
    private readonly Func<CancellationToken, ConfiguredValueTaskAwaitable> _successSignInFunc;

    public SignInViewModel(
        IUiAuthenticationService uiAuthenticationService,
        Func<CancellationToken, ConfiguredValueTaskAwaitable> successSignInFunc,
        ISettingsService<MelnikovSettings> settingsService
    )
    {
        _uiAuthenticationService = uiAuthenticationService;
        _successSignInFunc = successSignInFunc;
        _settingsService = settingsService;
    }

    [RelayCommand]
    private async Task InitializedAsync(CancellationToken ct)
    {
        await WrapCommandAsync(() => InitializedCore(ct).ConfigureAwait(false), ct);
    }

    private async ValueTask InitializedCore(CancellationToken ct)
    {
        var settings = await _settingsService.GetSettingsAsync(ct);

        if (!settings.Token.IsNullOrWhiteSpace())
        {
            IsAvailableOffline = true;
            _uiAuthenticationService.Login(settings.Token);
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
        var response = await _uiAuthenticationService.GetAsync(CreateManisGetRequest(), ct);

        if (await UiHelper.CheckValidationErrorsAsync(response, ct))
        {
            if (IsRememberMe)
            {
                await _settingsService.SaveSettingsAsync(
                    new() { Token = response.SignIns[LoginOrEmail].Token },
                    ct
                );
            }
            else
            {
                await _settingsService.SaveSettingsAsync(new() { Token = string.Empty }, ct);
            }

            await _successSignInFunc.Invoke(ct);
        }
    }

    private ManisGetRequest CreateManisGetRequest()
    {
        return new() { SignIns = new() { { LoginOrEmail, Password } } };
    }
}

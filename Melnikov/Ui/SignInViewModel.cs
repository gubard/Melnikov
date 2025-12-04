using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inanna.Helpers;
using Inanna.Models;
using Inanna.Services;
using Manis.Contract.Models;
using Melnikov.Services;

namespace Melnikov.Ui;

public partial class SignInViewModel : ViewModelBase, INonHeader
{
    [ObservableProperty] private string _loginOrEmail = string.Empty;
    [ObservableProperty] private string _password = string.Empty;

    private readonly IUiAuthenticationService _uiAuthenticationService;
    private readonly Func<CancellationToken, ValueTask> _successSignInFunc;
    private readonly Func<CancellationToken, ValueTask> _offlineSignInFunc;

    public SignInViewModel(IUiAuthenticationService uiAuthenticationService, Func<CancellationToken, ValueTask> successSignInFunc, Func<CancellationToken, ValueTask> offlineSignInFunc)
    {
        _uiAuthenticationService = uiAuthenticationService;
        _successSignInFunc = successSignInFunc;
        _offlineSignInFunc = offlineSignInFunc;
    }

    [RelayCommand]
    private async Task OfflineAsync(CancellationToken ct)
    {
        await WrapCommand(async () => await _offlineSignInFunc.Invoke(ct));
    }

    [RelayCommand]
    private async Task SignInAsync(CancellationToken ct)
    {
        await WrapCommand(async () =>
        {
            if (await UiHelper.CheckValidationErrorsAsync(_uiAuthenticationService.GetAsync(CreateManisGetRequest(), ct)))
            {
                await _successSignInFunc.Invoke(ct);
            }
        });
    }

    private ManisGetRequest CreateManisGetRequest()
    {
        return new()
        {
            SignIns = new()
            {
                {
                    LoginOrEmail, Password
                },
            },
        };
    }
}
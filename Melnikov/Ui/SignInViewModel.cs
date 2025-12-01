using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inanna.Helpers;
using Inanna.Models;
using Inanna.Services;
using Manis.Contract.Models;
using Manis.Contract.Services;

namespace Melnikov.Ui;

public partial class SignInViewModel : ViewModelBase, INonHeader
{
    [ObservableProperty] private string _loginOrEmail = string.Empty;
    [ObservableProperty] private string _password = string.Empty;

    private readonly IManisService _manisService;
    private readonly Func<CancellationToken, Task> _successSignInFunc;

    public SignInViewModel(IManisService manisService, Func<CancellationToken, Task> successSignInFunc)
    {
        _manisService = manisService;
        _successSignInFunc = successSignInFunc;
    }

    [RelayCommand]
    private Task SignInAsync(CancellationToken ct)
    {
        return WrapCommand(async () =>
        {
            if (await UiHelper.CheckValidationErrors(_manisService.GetAsync(CreateManisGetResponse(), ct)))
            {
                await _successSignInFunc.Invoke(ct);
            }
        });
    }

    private ManisGetRequest CreateManisGetResponse()
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
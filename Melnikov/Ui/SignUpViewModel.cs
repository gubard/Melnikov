using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gaia.Errors;
using Inanna.Helpers;
using Inanna.Models;
using Inanna.Services;
using Manis.Contract.Models;
using Manis.Contract.Services;

namespace Melnikov.Ui;

public partial class SignUpViewModel : ViewModelBase, INonHeader
{
    [ObservableProperty] private string _login = string.Empty;
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _repeatPassword = string.Empty;

    private readonly IManisService _manisService;

    public SignUpViewModel(IManisService manisService, IManisValidator manisValidator)
    {
        _manisService = manisService;

        SetValidation(nameof(Login), () => manisValidator.Validate(Login, nameof(Login)));
        SetValidation(nameof(Email), () => manisValidator.Validate(Email, nameof(Email)));
        SetValidation(nameof(Password), () => manisValidator.Validate(Password, nameof(Password)));

        SetValidation(nameof(RepeatPassword), () =>
        {
            if (RepeatPassword != Password)
            {
                return [new PropertyNotEqualValidationError(nameof(Password), nameof(RepeatPassword))];
            }

            return [];
        });
    }

    [RelayCommand]
    private Task SignUpAsync(CancellationToken ct)
    {
        return WrapCommand(async () =>
        {
            if (await UiHelper.CheckValidationErrors(_manisService.PostAsync(CreateManisPostRequest(), ct)))
            {
                await UiHelper.NavigateToAsync<SignInViewModel>(ct);
            }
        });
    }

    private ManisPostRequest CreateManisPostRequest()
    {
        return new()
        {
            CreateUsers =
            [
                new()
                {
                    Email = Email,
                    Login = Login,
                    Password = Password,
                },
            ],
        };
    }
}
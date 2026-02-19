using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gaia.Models;
using Gaia.Services;
using Inanna.Helpers;
using Inanna.Models;
using Inanna.Services;
using Manis.Contract.Models;
using Manis.Contract.Services;

namespace Melnikov.Ui;

public sealed partial class SignUpViewModel : ViewModelBase, INonHeader, INonNavigate, INonStatusBar
{
    [ObservableProperty]
    private string _login = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _repeatPassword = string.Empty;

    private readonly IAuthenticationService _authenticationService;

    public SignUpViewModel(
        IAuthenticationService authenticationService,
        IAuthenticationValidator authenticationValidator
    )
    {
        _authenticationService = authenticationService;

        SetValidation(nameof(Login), () => authenticationValidator.Validate(Login, nameof(Login)));
        SetValidation(nameof(Email), () => authenticationValidator.Validate(Email, nameof(Email)));

        SetValidation(
            nameof(Password),
            () => authenticationValidator.Validate(Password, nameof(Password))
        );

        SetValidation(
            nameof(RepeatPassword),
            () =>
            {
                if (RepeatPassword != Password)
                {
                    return
                    [
                        new PropertyNotEqualValidationError(
                            nameof(Password),
                            nameof(RepeatPassword)
                        ),
                    ];
                }

                return [];
            }
        );
    }

    [RelayCommand]
    private async Task SignUpAsync(CancellationToken ct)
    {
        await WrapCommandAsync(() => SignUpCore(ct).ConfigureAwait(false), ct);
    }

    private async ValueTask<IValidationErrors> SignUpCore(CancellationToken ct)
    {
        var response = await _authenticationService.PostAsync(
            Guid.NewGuid(),
            CreateManisPostRequest(),
            ct
        );

        if (response.ValidationErrors.Count != 0)
        {
            return response;
        }

        await UiHelper.NavigateToAsync<SignInViewModel>(ct);

        return response;
    }

    private ManisPostRequest CreateManisPostRequest()
    {
        return new()
        {
            CreateUsers =
            [
                new()
                {
                    Email = Email.Trim(),
                    Login = Login.Trim(),
                    Password = Password,
                },
            ],
        };
    }
}

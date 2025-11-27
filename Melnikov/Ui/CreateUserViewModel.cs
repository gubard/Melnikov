using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gaia.Errors;
using Inanna.Models;
using Manis.Contract.Services;

namespace Melnikov.Ui;

public partial class CreateUserViewModel : ViewModelBase
{
    [ObservableProperty] private string _login = string.Empty;
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _repeatPassword = string.Empty;

    private readonly IManisService _manisService;

    public CreateUserViewModel(object header, IManisService manisService, IManisValidator manisValidator)
    {
        Header = header;
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

    public object Header { get; }

    [RelayCommand]
    private Task CreateUserAsync(CancellationToken ct)
    {
        return WrapCommand(async () => await _manisService.PostAsync(new()
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
        }, ct));
    }
}
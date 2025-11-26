using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gaia.Errors;
using Gaia.Extensions;
using Gaia.Helpers;
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

    public CreateUserViewModel(object header, IManisService manisService)
    {
        Header = header;
        _manisService = manisService;

        SetValidation(nameof(Login), () =>
        {
            if (Login.IsNullOrWhiteSpace())
            {
                return [new PropertyEmptyValidationError(nameof(Login))];
            }

            if (Login.Length > 255)
            {
                return [new PropertyMaxSizeValidationError(nameof(Login), (ulong)Login.Length, 255)];
            }

            if (Login.Length < 3)
            {
                return [new PropertyMinSizeValidationError(nameof(Login), (ulong)Login.Length, 3)];
            }

            var index = Login.IndexOfAnyExcept(StringHelper.ValidLoginSearch);

            if (index >= 0)
            {
                return [new PropertyContainsInvalidValueValidationError<char>(nameof(Login), Login[index], StringHelper.ValidLoginChar.ToCharArray())];
            }

            return [];
        });

        SetValidation(nameof(Email), () =>
        {
            if (Email.IsNullOrWhiteSpace())
            {
                return [new PropertyEmptyValidationError(nameof(Email))];
            }

            if (!Email.IsEmail())
            {
                return [new PropertyInvalidValidationError(nameof(Email))];
            }

            if (Email.Length > 255)
            {
                return [new PropertyMaxSizeValidationError(nameof(Email), (ulong)Email.Length, 255)];
            }

            if (Email.Length < 5)
            {
                return [new PropertyMinSizeValidationError(nameof(Email), (ulong)Email.Length, 5)];
            }

            return [];
        });

        SetValidation(nameof(Password), () =>
        {
            if (Password.IsNullOrWhiteSpace())
            {
                return [new PropertyEmptyValidationError(nameof(Password))];
            }

            if (Password.Length > 512)
            {
                return [new PropertyMaxSizeValidationError(nameof(Password), (ulong)Password.Length, 512)];
            }

            if (Password.Length < 8)
            {
                return [new PropertyMinSizeValidationError(nameof(Password), (ulong)Password.Length, 5)];
            }

            return [];
        });

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
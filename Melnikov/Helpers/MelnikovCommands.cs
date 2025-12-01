using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Inanna.Helpers;
using Inanna.Services;
using Manis.Contract.Services;
using Melnikov.Ui;

namespace Melnikov.Helpers;

public static class MelnikovCommands
{
    static MelnikovCommands()
    {
        NavigateToSignUpViewCommand = new AsyncRelayCommand(UiHelper.NavigateToAsync<SignUpViewModel>);
        NavigateToSignInViewCommand = new AsyncRelayCommand(UiHelper.NavigateToAsync<SignInViewModel>);
    }

    public static readonly ICommand NavigateToSignUpViewCommand;
    public static readonly ICommand NavigateToSignInViewCommand;
}
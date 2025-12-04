using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Inanna.Helpers;
using Melnikov.Ui;

namespace Melnikov.Helpers;

public static class MelnikovCommands
{
    static MelnikovCommands()
    {
        NavigateToSignUpViewCommand = UiHelper.CreateCommand(async ct => await UiHelper.NavigateToAsync<SignUpViewModel>(ct));
        NavigateToSignInViewCommand = UiHelper.CreateCommand(async ct => await UiHelper.NavigateToAsync<SignInViewModel>(ct));
    }

    public static readonly ICommand NavigateToSignUpViewCommand;
    public static readonly ICommand NavigateToSignInViewCommand;
}
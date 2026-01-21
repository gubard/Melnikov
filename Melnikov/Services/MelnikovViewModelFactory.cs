using System.Runtime.CompilerServices;
using Gaia.Services;
using Inanna.Models;
using Melnikov.Ui;

namespace Melnikov.Services;

public interface IMelnikovViewModelFactory
{
    SignInViewModel CreateSignIn(
        Func<CancellationToken, ConfiguredValueTaskAwaitable> successSignInFunc
    );
}

public class MelnikovViewModelFactory : IMelnikovViewModelFactory
{
    public MelnikovViewModelFactory(
        IUiAuthenticationService uiAuthenticationService,
        IObjectStorage objectStorage,
        AppState appState
    )
    {
        _uiAuthenticationService = uiAuthenticationService;
        _objectStorage = objectStorage;
        _appState = appState;
    }

    public SignInViewModel CreateSignIn(
        Func<CancellationToken, ConfiguredValueTaskAwaitable> successSignInFunc
    )
    {
        return new(_uiAuthenticationService, successSignInFunc, _objectStorage, _appState);
    }

    private readonly IUiAuthenticationService _uiAuthenticationService;
    private readonly IObjectStorage _objectStorage;
    private readonly AppState _appState;
}

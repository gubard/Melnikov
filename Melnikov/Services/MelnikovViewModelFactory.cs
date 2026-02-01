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
        IAuthenticationUiService authenticationUiService,
        IObjectStorage objectStorage,
        AppState appState
    )
    {
        _authenticationUiService = authenticationUiService;
        _objectStorage = objectStorage;
        _appState = appState;
    }

    public SignInViewModel CreateSignIn(
        Func<CancellationToken, ConfiguredValueTaskAwaitable> successSignInFunc
    )
    {
        return new(_authenticationUiService, successSignInFunc, _objectStorage, _appState);
    }

    private readonly IAuthenticationUiService _authenticationUiService;
    private readonly IObjectStorage _objectStorage;
    private readonly AppState _appState;
}

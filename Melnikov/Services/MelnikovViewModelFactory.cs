using System.Runtime.CompilerServices;
using Gaia.Services;
using Inanna.Models;
using Inanna.Services;
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
        AppState appState,
        IServiceController serviceController
    )
    {
        _authenticationUiService = authenticationUiService;
        _objectStorage = objectStorage;
        _appState = appState;
        _serviceController = serviceController;
    }

    public SignInViewModel CreateSignIn(
        Func<CancellationToken, ConfiguredValueTaskAwaitable> successSignInFunc
    )
    {
        return new(
            _authenticationUiService,
            successSignInFunc,
            _objectStorage,
            _appState,
            _serviceController
        );
    }

    private readonly IAuthenticationUiService _authenticationUiService;
    private readonly IObjectStorage _objectStorage;
    private readonly AppState _appState;
    private readonly IServiceController _serviceController;
}

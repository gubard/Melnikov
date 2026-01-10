using System.Runtime.CompilerServices;
using Gaia.Services;
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
        IObjectStorage objectStorage
    )
    {
        _uiAuthenticationService = uiAuthenticationService;
        _objectStorage = objectStorage;
    }

    public SignInViewModel CreateSignIn(
        Func<CancellationToken, ConfiguredValueTaskAwaitable> successSignInFunc
    )
    {
        return new(_uiAuthenticationService, successSignInFunc, _objectStorage);
    }

    private readonly IUiAuthenticationService _uiAuthenticationService;
    private readonly IObjectStorage _objectStorage;
}

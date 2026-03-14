using System.Runtime.CompilerServices;
using Gaia.Services;
using Inanna.Models;
using Inanna.Services;
using Manis.Contract.Services;
using Melnikov.Ui;

namespace Melnikov.Services;

public interface IMelnikovViewModelFactory
{
    SignUpViewModel CreateSignUp();

    SignInViewModel CreateSignIn(
        Func<CancellationToken, ConfiguredValueTaskAwaitable> successSignInFunc
    );
}

public sealed class MelnikovViewModelFactory : IMelnikovViewModelFactory
{
    public MelnikovViewModelFactory(Gaia.Services.IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public SignUpViewModel CreateSignUp()
    {
        return new(
            _serviceProvider.GetService<IAuthenticationService>(),
            _serviceProvider.GetService<IAuthenticationValidator>(),
            _serviceProvider.GetService<InannaCommands>(),
            _serviceProvider.GetService<ISafeExecuteWrapper>(),
            _serviceProvider.GetService<INavigator>(),
            _serviceProvider
        );
    }

    public SignInViewModel CreateSignIn(
        Func<CancellationToken, ConfiguredValueTaskAwaitable> successSignInFunc
    )
    {
        return new(
            _serviceProvider.GetService<IAuthenticationUiService>(),
            successSignInFunc,
            _serviceProvider.GetService<IObjectStorage>(),
            _serviceProvider.GetService<AppState>(),
            _serviceProvider.GetService<IServiceController>(),
            _serviceProvider.GetService<InannaCommands>(),
            _serviceProvider.GetService<ISafeExecuteWrapper>()
        );
    }

    private readonly Gaia.Services.IServiceProvider _serviceProvider;
}

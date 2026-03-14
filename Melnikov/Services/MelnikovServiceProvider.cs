using Jab;

namespace Melnikov.Services;

[ServiceProviderModule]
[Singleton(typeof(IAuthenticationUiService), typeof(AuthenticationUiService))]
[Singleton(typeof(IMelnikovViewModelFactory), typeof(MelnikovViewModelFactory))]
public interface IMelnikovServiceProvider;

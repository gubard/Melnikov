using Jab;

namespace Melnikov.Services;

[ServiceProviderModule]
[Singleton(typeof(IAuthenticationUiService), typeof(AuthenticationUiService))]
public interface IMelnikovServiceProvider;

using Microsoft.Extensions.Logging;

namespace Melnikov.Helpers;

public static partial class MelnikovLog
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Logout")]
    public static partial void Logout(this ILogger logger);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "Login {Id}")]
    public static partial void Login(this ILogger logger, Guid id);
}

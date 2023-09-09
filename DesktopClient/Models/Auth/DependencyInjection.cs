using Microsoft.Extensions.DependencyInjection;

namespace DesktopClient.Models.Auth;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomAuthStateProvider(this IServiceCollection services) =>
        services.AddSingleton<AuthorizationControllerAccessor>();
}
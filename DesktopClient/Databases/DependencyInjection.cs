using DesktopClient.Databases.MockDb;
using Microsoft.Extensions.DependencyInjection;

namespace DesktopClient.Databases;

public static class DependencyInjection
{
    public static IServiceCollection AddRamDb(this IServiceCollection services) =>
        services.AddSingleton<IDatabase, RamDb>();
}
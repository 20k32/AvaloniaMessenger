using Microsoft.Extensions.DependencyInjection;
using Shared.Databases.MockDb;

namespace Shared.Databases;

public static class DependencyInjection
{
    public static IServiceCollection AddRamDb(this IServiceCollection services) =>
        services.AddSingleton<IDatabase, RamDb>();
}
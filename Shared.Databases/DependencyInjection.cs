using Microsoft.Extensions.DependencyInjection;
using Shared.Databases.MockDb;
using Shared.Databases.MongoDB;

namespace Shared.Databases;

public static class DependencyInjection
{
    public static IServiceCollection AddRamDb(this IServiceCollection services) =>
        services.AddSingleton<IDatabase, RamDb>();

    public static IServiceCollection AddMongoDb(this IServiceCollection services) =>
        services.AddSingleton<IDatabase, MongoDb>();
}
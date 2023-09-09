#region

using System;
using DesktopClient.Models.Auth;
using DesktopClient.ViewModels;
using DesktopClient.Views;
using Microsoft.Extensions.DependencyInjection;
using Shared.Databases;

#endregion

namespace DesktopClient
{
    public static class DependencyInjection
    {
        public static IServiceProvider ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddCustomAuthStateProvider();
            services.AddMongoDb();

            /*var bob = new UsersDbUserEntry("@bob", "12345", "Bobby");
            var yegor = new UsersDbUserEntry("@yegor", "12345", "Yegor");
            var alex = new UsersDbUserEntry("@alex", "12345", "Alex");
            var admin = new UsersDbUserEntry("@admin", "12345", "Admin");
            
            admin.Friends.Add(bob);
            admin.Friends.Add(yegor);*/
            
            var database = services.BuildServiceProvider().GetRequiredService<IDatabase>();
            
            /*database.AddUserSync(bob);
            database.AddUserSync(yegor);
            database.AddUserSync(alex);
            database.AddUserSync(admin);*/
            
            /*var user = database.GetUserByUserNameSync("@admin");
            
            services.AddSingleton(user!);*/
            
            return services.BuildServiceProvider();
        }
    }
}

using Avalonia;
using DesktopClient.ViewModels;
using DesktopClient.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopClient.Models.Auth;
using DesktopClient.Views;
using DesktopClient.Models.Auth;
using DynamicData;
using Shared.Databases;
using Shared.Databases.DTOs;

namespace DesktopClient
{
    public static class DependencyInjection
    {
        public static IServiceProvider ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
            
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
            
            var user = database.GetUserByUserNameSync("@admin");
            
            services.AddSingleton(user!);
            
            return services.BuildServiceProvider();
        }
    }
}

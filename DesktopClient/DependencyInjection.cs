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
            
            //current user of a programm
            var user = new UsersDbUserEntry("@admin", "11111", "Admin");
            
            
            services.AddMongoDb();
            var database = services.BuildServiceProvider().GetRequiredService<IDatabase>();
            
            database.AddUserSync(user);
            
            database.AddUserSync(new ("@yegor", "12345", "Yegorchik"));
            database.AddUserSync(new("@bob", "12345", "Bob"));
            database.AddUserSync(new("@alex", "12345", "Alex"));
            
            user.Friends.Add(database.GetUserByUserNameSync("@yegor")!);
            user.Friends.Add(database.GetUserByUserNameSync("@bob")!);
            user.Friends.Add(database.GetUserByUserNameSync("@alex")!);


            services.AddSingleton(user);
            return services.BuildServiceProvider();
        }
    }
}

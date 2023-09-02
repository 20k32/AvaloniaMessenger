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
            
            var user = new UsersDbUserEntry("@admin", "11111", "Admin");
            
            services.AddRamDb();
            var _usersDb = services.BuildServiceProvider().GetRequiredService<IDatabase>();
            
            // todo: crate users, fill db with start values
            
            /*user.Friends.Add(_usersDb.GetUserByUserName("@yegor")!);
            user.Friends.Add(_usersDb.GetUserByUserName("@bob")!);
            user.Friends.Add(_usersDb.GetUserByUserName("@alex")!);

            _usersDb.AddUser(user);*/
            
            services.AddSingleton(user);
            return services.BuildServiceProvider();
        }
    }
}

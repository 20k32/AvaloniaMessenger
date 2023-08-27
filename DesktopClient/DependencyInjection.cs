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

namespace DesktopClient
{
    public static class DependencyInjection
    {
        public static IServiceProvider ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton(new User("Yegorchik", "@yegorchik", "12345"));
            return services.BuildServiceProvider();
        }
    }
}


namespace AspServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var startup = new Startup();
            startup.ConfigrueServices(builder.Services);
            
            var app = builder.Build();
            startup.Configure(app, app.Environment);

            app.Run();
        }
    }
}
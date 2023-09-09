using AspServer.Hubs.ChatHub;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Shared.Databases;

namespace AspServer;

public class Startup
{
    public void ConfigrueServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                
                options.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["token"];
                        if (!string.IsNullOrWhiteSpace(accessToken)
                            && context.Request.Path.StartsWithSegments("/chat"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorizationCore();

        services.AddCors(setup => setup.AddPolicy("DefaultPolicy", policy =>
        {
            policy.SetIsOriginAllowed(origin => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }));

        services.AddSignalR();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMongoDb();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        /*if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }*/

        app.UseCors("DefaultPolicy");
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{Controller=SingIn}/{action=Authorize}");

            endpoints.MapHub<ChatHub>("/chat");
        });

    }
}
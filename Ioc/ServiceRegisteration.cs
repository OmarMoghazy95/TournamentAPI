using Tournament.Api.Infrastructure.Extensions;

namespace Tournament.Api.Ioc;

public static partial class IServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.RegisterDbContext();
        services.AddCors(options =>
        {
            options.AddPolicy("ClientApp", policy =>
            {
                policy
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

    }
}

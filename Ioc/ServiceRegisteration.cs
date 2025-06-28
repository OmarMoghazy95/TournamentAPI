using Tournament.Api.Application.Services;
using Tournament.Api.Infrastructure.Extensions;

namespace Tournament.Api.Ioc;

public static partial class IServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.RegisterDbContext();
        services.AddScoped<ITournamentService, TournamentService>();
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

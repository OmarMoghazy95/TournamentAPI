using Microsoft.EntityFrameworkCore;
using Tournament.Api.Infrastructure.Context;
using SQLitePCL;

namespace Tournament.Api.Infrastructure.Extensions;

public static partial class IServiceCollectionExtensions
{

    public static void RegisterDbContext(this IServiceCollection services)
    {
        Batteries.Init();
        services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=Tournament.db"));

    }
}

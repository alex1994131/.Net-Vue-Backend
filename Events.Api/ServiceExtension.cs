using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventsServerSide
{
    public static class ServiceExtension
    { 
   public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });
        });
    }

    public static void ConfigureIISService(this IServiceCollection services)
    {
        services.Configure<IISOptions>(options => { });
    }
}
}

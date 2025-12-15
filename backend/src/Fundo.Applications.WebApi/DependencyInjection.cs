using Microsoft.Extensions.DependencyInjection;

namespace Fundo.Applications.WebApi
{
    public static class DependencyInjection
    {
        public const string DefaultCorsPolicy = "DefaultCorsPolicy";

        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicy, policy =>
                {
                    policy
                        .WithOrigins(
                            "http://localhost:4200",
                            "http://localhost:8080"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            return services;
        }
    }
}
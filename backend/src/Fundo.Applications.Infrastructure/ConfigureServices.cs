using Fundo.Applications.Application.Interfaces;
using Fundo.Applications.Infrastructure.Data;
using Fundo.Applications.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fundo.Applications.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            bool isTesting)
        {
            if (!isTesting)
            {
                var connectionString = configuration.GetConnectionString("LoanDatabase");

                services.AddDbContext<LoanDbContext>(options =>
                {
                    options.UseSqlServer(
                        connectionString,
                        sql => sql.MigrationsAssembly(typeof(LoanDbContext).Assembly.FullName));
                });
            }

            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
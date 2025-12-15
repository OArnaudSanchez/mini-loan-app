using Fundo.Applications.Application;
using Fundo.Applications.Infrastructure;
using Fundo.Applications.Infrastructure.Data;
using Fundo.Applications.Infrastructure.Data.Seed;
using Fundo.Applications.WebApi.MIddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fundo.Applications.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApplication();
            services.AddInfrastructure(Configuration, isTesting: Environment.IsEnvironment("Testing"));
            services.AddWebApi();

            //TODO: Pass a postman collection to test the API
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                using var scope = app.ApplicationServices.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<LoanDbContext>();

                context.Database.Migrate();
                LoanDbSeeder.SeedAsync(context).GetAwaiter().GetResult();
            }

            app.UseCors(DependencyInjection.DefaultCorsPolicy);
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
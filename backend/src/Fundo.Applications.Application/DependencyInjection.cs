using FluentValidation;
using Fundo.Applications.Application.Behaviors;
using Fundo.Applications.Application.Features.Loans.Commands.CreateLoan;
using Fundo.Applications.Application.Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fundo.Applications.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(
                    typeof(CreateLoanCommand).Assembly));

            services.AddValidatorsFromAssembly(
                typeof(CreateLoanValidator).Assembly);

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));

            services.AddAutoMapper(
                AutoMapperConfiguration.Configure,
                typeof(AutoMapperProfile).Assembly);

            return services;
        }
    }
}
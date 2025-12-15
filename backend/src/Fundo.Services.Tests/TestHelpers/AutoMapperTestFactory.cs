using AutoMapper;
using Fundo.Applications.Application.Mappings;
using Microsoft.Extensions.Logging;

namespace Fundo.Services.Tests.TestHelpers
{
    public static class AutoMapperTestFactory
    {
        public static IMapper Create()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddDebug();
                builder.AddConsole();
            });

            var expression = new MapperConfigurationExpression();

            expression.AddProfile<AutoMapperProfile>();

            var configuration = new MapperConfiguration(expression, loggerFactory);

            configuration.AssertConfigurationIsValid();

            return configuration.CreateMapper();
        }
    }
}
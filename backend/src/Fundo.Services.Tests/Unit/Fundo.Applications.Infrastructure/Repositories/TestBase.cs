using Fundo.Applications.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fundo.Services.Tests.Unit.Fundo.Applications.Infrastructure.Repositories
{
    public abstract class TestBase
    {
        protected LoanDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<LoanDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new LoanDbContext(options);
        }
    }
}
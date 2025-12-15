using FluentAssertions;
using Fundo.Applications.Domain.Entities;
using Fundo.Applications.Infrastructure.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Unit.Fundo.Applications.Infrastructure.Repositories
{
    public class UnitOfWorkTests : TestBase
    {
        [Fact]
        public async Task SaveChangesAsync_ShouldPersistChanges()
        {
            // Arrange
            using var context = CreateDbContext();
            var unitOfWork = new UnitOfWork(context);

            context.Loans.Add(new Loan(1000, "Pedro"));

            // Act
            await unitOfWork.SaveChangesAsync();

            // Assert
            context.Loans.Should().HaveCount(1);
        }
    }
}
using FluentAssertions;
using Fundo.Applications.Domain.Entities;
using Fundo.Applications.Infrastructure.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Unit.Fundo.Applications.Infrastructure.Repositories
{
    public class LoanRepositoryTests : TestBase
    {
        [Fact]
        public async Task GetLoansAsync_ShouldReturnAllLoans()
        {
            // Arrange
            using var context = CreateDbContext();
            context.Loans.Add(new Loan(1000, "User A"));
            context.Loans.Add(new Loan(2000, "User B"));
            await context.SaveChangesAsync();

            var repository = new LoanRepository(context);

            // Act
            var result = await repository.GetLoansAsync();

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetLoanByIdAsync_ShouldReturnLoan_WhenExists()
        {
            // Arrange
            using var context = CreateDbContext();
            var loan = new Loan(1500, "Maria");
            context.Loans.Add(loan);
            await context.SaveChangesAsync();

            var repository = new LoanRepository(context);

            // Act
            var result = await repository.GetLoanByIdAsync(loan.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(loan.Id);
        }

        [Fact]
        public async Task GetLoanByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new LoanRepository(context);

            // Act
            var result = await repository.GetLoanByIdAsync("non-existing-id");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateLoanAsync_ShouldAddLoanToContext()
        {
            // Arrange
            using var context = CreateDbContext();
            var repository = new LoanRepository(context);
            var loan = new Loan(1200, "Carlos");

            // Act
            await repository.CreateLoanAsync(loan);
            await context.SaveChangesAsync();

            // Assert
            context.Loans.Should().HaveCount(1);
        }

        [Fact]
        public async Task UpdateLoan_ShouldUpdateLoan()
        {
            // Arrange
            using var context = CreateDbContext();
            var loan = new Loan(1000, "Ana");
            context.Loans.Add(loan);
            await context.SaveChangesAsync();

            var repository = new LoanRepository(context);

            // Act
            loan.ApplyPayment(200);
            repository.UpdateLoan(loan);
            await context.SaveChangesAsync();

            // Assert
            var updatedLoan = await context.Loans.FindAsync(loan.Id);
            updatedLoan!.CurrentBalance.Should().Be(800);
        }
    }
}
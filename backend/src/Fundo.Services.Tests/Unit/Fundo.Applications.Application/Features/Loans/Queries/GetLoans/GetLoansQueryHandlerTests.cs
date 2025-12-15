using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Fundo.Applications.Application.DTOs;
using Fundo.Applications.Application.Features.Loans.Queries.GetLoans;
using Fundo.Applications.Application.Interfaces;
using Fundo.Applications.Domain.Entities;
using Fundo.Services.Tests.TestHelpers;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Unit.Fundo.Applications.Application.Features.Loans.Queries.GetLoans
{
    public class GetLoansQueryHandlerTests
    {
        private readonly Mock<ILoanRepository> _loanRepository = new();

        private readonly IMapper _mapper = AutoMapperTestFactory.Create();

        private Fixture _fixture = new Fixture();

        [Fact]
        public async Task Handle_ShouldReturnMappedLoans()
        {
            // Arrange
            var loans = _fixture.CreateMany<Loan>();
            var loanDtos = _fixture.CreateMany<LoanDto>();

            _loanRepository
                .Setup(r => r.GetLoansAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(loans);

            var handler = new GetLoansQueryHandler(
                _loanRepository.Object,
                _mapper);

            // Act
            var result = await handler.Handle(new GetLoansQuery(), CancellationToken.None);

            // Assert
            result.Should().HaveCount(loanDtos.Count());
        }
    }
}
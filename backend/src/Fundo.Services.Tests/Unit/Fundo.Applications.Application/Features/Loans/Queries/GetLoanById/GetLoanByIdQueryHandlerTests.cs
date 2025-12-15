using AutoMapper;
using FluentAssertions;
using Fundo.Applications.Application.Exceptions;
using Fundo.Applications.Application.Features.Loans.Queries.GetLoanById;
using Fundo.Applications.Application.Interfaces;
using Fundo.Applications.Domain.Entities;
using Fundo.Services.Tests.TestHelpers;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Unit.Fundo.Applications.Application.Features.Loans.Queries.GetLoanById
{
    public class GetLoanByIdQueryHandlerTests
    {
        private readonly Mock<ILoanRepository> _loanRepository = new();

        private readonly IMapper _mapper = AutoMapperTestFactory.Create();

        private readonly Mock<ILogger<GetLoanByIdQueryHandler>> _logger = new();

        [Fact]
        public async Task Handle_ShouldThrowNotFound_WhenLoanDoesNotExist()
        {
            // Arrange
            var query = new GetLoanByIdQuery("loan-id");

            _loanRepository
                .Setup(r => r.GetLoanByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Loan?)null);

            var handler = new GetLoanByIdQueryHandler(
                _loanRepository.Object,
                _mapper,
                _logger.Object);

            // Act
            var act = () => handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldReturnLoanDto_WhenFound()
        {
            // Arrange
            var loan = new Loan(1200, "Ana");
            var query = new GetLoanByIdQuery(loan.Id);

            _loanRepository
                .Setup(r => r.GetLoanByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(loan);

            var handler = new GetLoanByIdQueryHandler(
                _loanRepository.Object,
                _mapper,
                _logger.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
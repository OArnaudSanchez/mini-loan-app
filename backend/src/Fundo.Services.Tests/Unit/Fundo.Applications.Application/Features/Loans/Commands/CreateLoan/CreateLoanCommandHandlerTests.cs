using AutoMapper;
using FluentAssertions;
using Fundo.Applications.Application.Features.Loans.Commands.CreateLoan;
using Fundo.Applications.Application.Interfaces;
using Fundo.Applications.Domain.Entities;
using Fundo.Services.Tests.TestHelpers;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Unit.Fundo.Applications.Application.Features.Loans.Commands.CreateLoan
{
    public class CreateLoanCommandHandlerTests
    {
        private readonly Mock<ILoanRepository> _loanRepository = new();

        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        private readonly IMapper _mapper = AutoMapperTestFactory.Create();

        private readonly Mock<ILogger<CreateLoanCommandHandler>> _logger = new();

        [Fact]
        public async Task Handle_ShouldCreateLoanAndReturnDto()
        {
            // Arrange
            var command = new CreateLoanCommand(1500, "Maria Silva");

            var loan = new Loan(command.Amount, command.ApplicantName);

            var handler = new CreateLoanCommandHandler(
                _logger.Object,
                _loanRepository.Object,
                _mapper,
                _unitOfWork.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            _loanRepository.Verify(
                r => r.CreateLoanAsync(It.IsAny<Loan>(), It.IsAny<CancellationToken>()), Times.Once);

            _unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
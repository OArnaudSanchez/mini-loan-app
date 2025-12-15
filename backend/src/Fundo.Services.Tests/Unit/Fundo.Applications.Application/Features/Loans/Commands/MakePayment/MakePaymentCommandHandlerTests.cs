using AutoMapper;
using FluentAssertions;
using Fundo.Applications.Application.Exceptions;
using Fundo.Applications.Application.Features.Loans.Commands.MakePayment;
using Fundo.Applications.Application.Interfaces;
using Fundo.Applications.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Unit.Fundo.Applications.Application.Features.Loans.Commands.MakePayment
{
    public class MakePaymentCommandHandlerTests
    {
        private readonly Mock<ILoanRepository> _loanRepository = new();

        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        private readonly Mock<IMapper> _mapper = new();

        private readonly Mock<ILogger<MakePaymentCommandHandler>> _logger = new();

        [Fact]
        public async Task Handle_ShouldThrowNotFound_WhenLoanDoesNotExist()
        {
            // Arrange
            var command = new MakePaymentCommand("loan-id", 100);

            _loanRepository
                .Setup(r => r.GetLoanByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Loan?)null);

            var handler = new MakePaymentCommandHandler(
                _loanRepository.Object,
                _unitOfWork.Object,
                _mapper.Object,
                _logger.Object);

            // Act
            var act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenPaymentExceedsBalance()
        {
            // Arrange
            var loan = new Loan(500, "Pedro");
            var command = new MakePaymentCommand(loan.Id, 600);

            _loanRepository
                .Setup(r => r.GetLoanByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(loan);

            var handler = new MakePaymentCommandHandler(
                _loanRepository.Object,
                _unitOfWork.Object,
                _mapper.Object,
                _logger.Object);

            // Act
            var act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task Handle_ShouldApplyPayment_WhenValid()
        {
            // Arrange
            var loan = new Loan(1000, "Carlos");
            var command = new MakePaymentCommand(loan.Id, 200);

            _loanRepository
                .Setup(r => r.GetLoanByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(loan);

            var handler = new MakePaymentCommandHandler(
                _loanRepository.Object,
                _unitOfWork.Object,
                _mapper.Object,
                _logger.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            loan.CurrentBalance.Should().Be(800);
            _unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
using AutoMapper;
using Fundo.Applications.Application.DTOs;
using Fundo.Applications.Application.Exceptions;
using Fundo.Applications.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fundo.Applications.Application.Features.Loans.Commands.MakePayment
{
    public class MakePaymentCommandHandler : IRequestHandler<MakePaymentCommand, LoanDto>
    {
        private readonly ILoanRepository _loanRepository;

        private readonly ILogger<MakePaymentCommandHandler> _logger;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public MakePaymentCommandHandler(
            ILoanRepository loanRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<MakePaymentCommandHandler> logger)
        {
            _loanRepository = loanRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LoanDto> Handle(MakePaymentCommand request, CancellationToken cancellationToken)
        {
            var loan = await _loanRepository.GetLoanByIdAsync(request.Id, cancellationToken);

            if (loan is null)
            {
                _logger.LogWarning(
                    "Payment attempted on non-existing loan. LoanId: {LoanId}",
                    request.Id);

                throw new NotFoundException("Loan", request.Id);
            }

            if (request.Payment > loan.CurrentBalance)
            {
                _logger.LogWarning("Invalid payment. LoanId: {LoanId}, Payment: {Payment}, Balance: {Balance}",
                   loan.Id,
                   request.Payment,
                   loan.CurrentBalance);

                throw new ValidationException("Payment exceeds current loan balance.");
            }

            loan.ApplyPayment(request.Payment);

            _loanRepository.UpdateLoan(loan);

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Payment applied. LoanId: {LoanId}, NewBalance: {Balance}",
               loan.Id, loan.CurrentBalance);

            return _mapper.Map<LoanDto>(loan);
        }
    }
}
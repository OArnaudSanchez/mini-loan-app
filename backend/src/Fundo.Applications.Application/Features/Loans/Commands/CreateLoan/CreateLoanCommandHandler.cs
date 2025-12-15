using AutoMapper;
using Fundo.Applications.Application.DTOs;
using Fundo.Applications.Application.Interfaces;
using Fundo.Applications.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Fundo.Applications.Application.Features.Loans.Commands.CreateLoan
{
    public class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, LoanDto>
    {
        private readonly ILoanRepository _loanRepository;

        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public CreateLoanCommandHandler(
            ILogger<CreateLoanCommandHandler> logger,
            ILoanRepository loanRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoanDto> Handle(
            CreateLoanCommand request,
            CancellationToken cancellationToken)
        {
            var loan = new Loan(request.Amount, request.ApplicantName);

            await _loanRepository.CreateLoanAsync(loan, cancellationToken);

            await _unitOfWork.SaveChangesAsync();

            Log.Information("Created new loan with ID: {LoanId} and amount: {Amount} to: {ApplicantName}",
                loan.Id,
                loan.Amount,
                loan.ApplicantName);

            return _mapper.Map<Loan, LoanDto>(loan);
        }
    }
}
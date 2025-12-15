using AutoMapper;
using Fundo.Applications.Application.DTOs;
using Fundo.Applications.Application.Exceptions;
using Fundo.Applications.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fundo.Applications.Application.Features.Loans.Queries.GetLoanById
{
    public class GetLoanByIdQueryHandler : IRequestHandler<GetLoanByIdQuery, LoanDto>
    {
        private readonly ILoanRepository _loanRepository;

        private ILogger<GetLoanByIdQueryHandler> _logger;

        private readonly IMapper _mapper;

        public GetLoanByIdQueryHandler(
            ILoanRepository loanRepository,
            IMapper mapper,
            ILogger<GetLoanByIdQueryHandler> logger)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LoanDto> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
        {
            var loan = await _loanRepository.GetLoanByIdAsync(request.Id, cancellationToken);

            if (loan is null)
            {
                _logger.LogWarning("Loan not found. LoanId: {LoanId}", request.Id);

                throw new NotFoundException("Loan", request.Id);
            }

            _logger.LogInformation("Retrieved loan with ID {LoanId}", loan.Id);

            return _mapper.Map<LoanDto>(loan);
        }
    }
}
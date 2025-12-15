using AutoMapper;
using Fundo.Applications.Application.DTOs;
using Fundo.Applications.Application.Interfaces;
using MediatR;

namespace Fundo.Applications.Application.Features.Loans.Queries.GetLoans
{
    public class GetLoansQueryHandler : IRequestHandler<GetLoansQuery, IEnumerable<LoanDto>>
    {
        private readonly ILoanRepository _loanRepository;

        private readonly IMapper _mapper;

        public GetLoansQueryHandler(ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LoanDto>> Handle(
            GetLoansQuery request,
            CancellationToken cancellationToken)
        {
            var loans = await _loanRepository.GetLoansAsync(cancellationToken);
            var loansDto = _mapper.Map<IEnumerable<LoanDto>>(loans);
            return loansDto;
        }
    }
}
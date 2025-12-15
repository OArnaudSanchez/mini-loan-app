using Fundo.Applications.Application.DTOs;
using MediatR;

namespace Fundo.Applications.Application.Features.Loans.Queries.GetLoans
{
    public record GetLoansQuery : IRequest<IEnumerable<LoanDto>>;
}
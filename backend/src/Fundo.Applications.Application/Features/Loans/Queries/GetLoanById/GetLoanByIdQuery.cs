using Fundo.Applications.Application.DTOs;
using MediatR;

namespace Fundo.Applications.Application.Features.Loans.Queries.GetLoanById
{
    public record GetLoanByIdQuery(string Id) : IRequest<LoanDto>;
}
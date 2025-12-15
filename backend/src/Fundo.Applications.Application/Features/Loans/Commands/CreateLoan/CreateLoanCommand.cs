using Fundo.Applications.Application.DTOs;
using MediatR;

namespace Fundo.Applications.Application.Features.Loans.Commands.CreateLoan
{
    public record CreateLoanCommand(decimal Amount, string ApplicantName) : IRequest<LoanDto>;
}
using Fundo.Applications.Application.DTOs;
using MediatR;

namespace Fundo.Applications.Application.Features.Loans.Commands.MakePayment
{
    public record MakePaymentCommand(string Id, decimal Payment) : IRequest<LoanDto>;
}
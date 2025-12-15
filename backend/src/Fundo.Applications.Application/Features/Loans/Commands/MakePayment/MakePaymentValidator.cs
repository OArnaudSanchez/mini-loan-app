using FluentValidation;

namespace Fundo.Applications.Application.Features.Loans.Commands.MakePayment
{
    public class MakePaymentValidator : AbstractValidator<MakePaymentCommand>
    {
        public MakePaymentValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(x => x.Payment).NotEmpty().GreaterThan(0).WithMessage("Payment must be greater than zero");
        }
    }
}
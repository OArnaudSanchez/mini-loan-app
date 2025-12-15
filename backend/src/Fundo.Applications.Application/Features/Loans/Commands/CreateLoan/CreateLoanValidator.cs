using FluentValidation;

namespace Fundo.Applications.Application.Features.Loans.Commands.CreateLoan
{
    public class CreateLoanValidator : AbstractValidator<CreateLoanCommand>
    {
        public CreateLoanValidator()
        {
            RuleFor(x => x.Amount)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0");

            RuleFor(x => x.ApplicantName)
                .NotEmpty()
                .MinimumLength(10);
        }
    }
}
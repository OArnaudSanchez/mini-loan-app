using Fundo.Applications.Domain.Enums;

namespace Fundo.Applications.Domain.Entities
{
    public class Loan
    {
        public string Id { get; private set; }

        public decimal Amount { get; private set; }

        public decimal CurrentBalance { get; private set; }

        public string ApplicantName { get; private set; } = string.Empty;

        public LoanStatus Status { get; private set; }

        protected Loan()
        { }

        public Loan(decimal amount, string applicantName)
        {
            CreateLoan(amount, applicantName);
        }

        private void CreateLoan(decimal amount, string applicantName)
        {
            Id = Guid.NewGuid().ToString();
            Amount = amount;
            ApplicantName = applicantName;
            CurrentBalance = amount;
            Status = LoanStatus.Active;
        }

        public void ApplyPayment(decimal payment)
        {
            CurrentBalance -= payment;
            const decimal EMPTY_BALANCE = 0;

            if (CurrentBalance == EMPTY_BALANCE)
            {
                Status = LoanStatus.Paid;
            }
        }
    }
}
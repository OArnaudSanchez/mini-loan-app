using Fundo.Applications.Domain.Entities;

namespace Fundo.Applications.Application.Interfaces
{
    public interface ILoanRepository
    {
        Task<IEnumerable<Loan>> GetLoansAsync(CancellationToken cancellationToken = default);
        Task<Loan?> GetLoanByIdAsync(string id, CancellationToken cancellationToken = default);
        Task CreateLoanAsync(Loan loan, CancellationToken cancellationToken = default);
        void UpdateLoan(Loan loan);
    }
}

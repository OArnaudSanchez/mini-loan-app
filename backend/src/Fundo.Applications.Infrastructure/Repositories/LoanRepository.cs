using Fundo.Applications.Application.Interfaces;
using Fundo.Applications.Domain.Entities;
using Fundo.Applications.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fundo.Applications.Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LoanDbContext _context;

        public LoanRepository(LoanDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Loan>> GetLoansAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Loans.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Loan?> GetLoanByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _context.Loans.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task CreateLoanAsync(Loan loan, CancellationToken cancellationToken = default)
        {
            await _context.AddAsync(loan, cancellationToken);
        }

        public void UpdateLoan(Loan loan)
        {
            _context.Update(loan);
        }
    }
}
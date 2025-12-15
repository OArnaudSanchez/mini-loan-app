using Fundo.Applications.Application.Interfaces;
using Fundo.Applications.Infrastructure.Data;

namespace Fundo.Applications.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly LoanDbContext _context;

        public UnitOfWork(LoanDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
using Fundo.Applications.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fundo.Applications.Infrastructure.Data.Seed
{
    public class LoanDbSeeder
    {
        public static async Task SeedAsync(LoanDbContext context)
        {
            if (await context.Loans.AnyAsync())
                return;

            var loans = new List<Loan>();

            var loan1 = new Loan(1500m, "Maria Silva");
            loan1.ApplyPayment(1000m);
            loans.Add(loan1);

            var loan2 = new Loan(800m, "Juan Perez");
            loan2.ApplyPayment(800m);
            loans.Add(loan2);

            var loan3 = new Loan(2500m, "Ana Rodriguez");
            loans.Add(loan3);

            var loan4 = new Loan(1200m, "Carlos Méndez");
            loan4.ApplyPayment(200m);
            loan4.ApplyPayment(300m);
            loans.Add(loan4);

            var loan5 = new Loan(5000m, "Lucía Fernández");
            loan5.ApplyPayment(2500m);
            loan5.ApplyPayment(2500m);
            loans.Add(loan5);

            await context.Loans.AddRangeAsync(loans);
            await context.SaveChangesAsync();
        }
    }
}
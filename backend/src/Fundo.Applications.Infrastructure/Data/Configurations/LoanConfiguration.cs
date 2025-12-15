using Fundo.Applications.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fundo.Applications.Infrastructure.Data.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);
            builder.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();
            builder.Property(x => x.CurrentBalance)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.ApplicantName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Status).IsRequired();
        }
    }
}
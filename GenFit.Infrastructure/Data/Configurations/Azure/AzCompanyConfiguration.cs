using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenFit.Core.Entities.Azure;

namespace GenFit.Infrastructure.Data.Configurations.Azure;

public class AzCompanyConfiguration : IEntityTypeConfiguration<AzCompany>
{
    public void Configure(EntityTypeBuilder<AzCompany> builder)
    {
        builder.ToTable("AZ_COMPANIES");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nome)
            .HasColumnName("nome")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Cnpj)
            .HasColumnName("cnpj")
            .HasMaxLength(18);

        builder.Property(c => c.Setor)
            .HasColumnName("setor")
            .HasMaxLength(100);

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
    }
}


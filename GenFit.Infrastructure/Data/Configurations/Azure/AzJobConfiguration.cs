using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenFit.Core.Entities.Azure;

namespace GenFit.Infrastructure.Data.Configurations.Azure;

public class AzJobConfiguration : IEntityTypeConfiguration<AzJob>
{
    public void Configure(EntityTypeBuilder<AzJob> builder)
    {
        builder.ToTable("AZ_JOBS");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(j => j.Titulo)
            .HasColumnName("titulo")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(j => j.Descricao)
            .HasColumnName("descricao")
            .HasColumnType("nvarchar(max)");

        builder.Property(j => j.Salario)
            .HasColumnName("salario")
            .HasColumnType("decimal(10,2)");

        builder.Property(j => j.Localizacao)
            .HasColumnName("localizacao")
            .HasMaxLength(100);

        builder.Property(j => j.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(j => j.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasMany(j => j.Applications)
            .WithOne(a => a.Job)
            .HasForeignKey(a => a.JobId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


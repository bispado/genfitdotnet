using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenFit.Core.Entities.Azure;

namespace GenFit.Infrastructure.Data.Configurations.Azure;

public class AzApplicationConfiguration : IEntityTypeConfiguration<AzApplication>
{
    public void Configure(EntityTypeBuilder<AzApplication> builder)
    {
        builder.ToTable("AZ_APPLICATIONS");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(a => a.JobId)
            .HasColumnName("job_id")
            .IsRequired();

        builder.Property(a => a.CandidateName)
            .HasColumnName("candidate_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.CandidateEmail)
            .HasColumnName("candidate_email")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.Status)
            .HasColumnName("status")
            .HasMaxLength(50)
            .HasDefaultValue("Pendente");

        builder.Property(a => a.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasOne(a => a.Job)
            .WithMany(j => j.Applications)
            .HasForeignKey(a => a.JobId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


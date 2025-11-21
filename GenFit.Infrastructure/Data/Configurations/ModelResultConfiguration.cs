using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenFit.Core.Entities;

namespace GenFit.Infrastructure.Data.Configurations;

public class ModelResultConfiguration : IEntityTypeConfiguration<ModelResult>
{
    public void Configure(EntityTypeBuilder<ModelResult> builder)
    {
        builder.ToTable("MODEL_RESULTS");

        builder.HasKey(mr => mr.Id);
        builder.Property(mr => mr.Id).HasColumnName("id");

        builder.Property(mr => mr.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(mr => mr.JobId)
            .HasColumnName("job_id")
            .IsRequired();

        builder.Property(mr => mr.ScoreAfinidadeCultural)
            .HasColumnName("score_afinidade_cultural")
            .HasColumnType("NUMBER(5,2)");

        builder.Property(mr => mr.ScoreCompatibilidadeProfissional)
            .HasColumnName("score_compatibilidade_profissional")
            .HasColumnType("NUMBER(5,2)");

        builder.Property(mr => mr.RedFlags)
            .HasColumnName("red_flags")
            .HasMaxLength(1000);

        builder.Property(mr => mr.Recomendacao)
            .HasColumnName("recomendacao")
            .HasMaxLength(50);

        builder.Property(mr => mr.Detalhes)
            .HasColumnName("detalhes")
            .HasColumnType("CLOB");

        builder.Property(mr => mr.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("SYSDATE");

        builder.HasOne(mr => mr.User)
            .WithMany(u => u.ModelResults)
            .HasForeignKey(mr => mr.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mr => mr.Job)
            .WithMany(j => j.ModelResults)
            .HasForeignKey(mr => mr.JobId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

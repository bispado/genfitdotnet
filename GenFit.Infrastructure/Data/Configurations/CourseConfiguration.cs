using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenFit.Core.Entities;

namespace GenFit.Infrastructure.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("COURSES");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");

        builder.Property(c => c.Nome)
            .HasColumnName("nome")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(500);

        builder.Property(c => c.Categoria)
            .HasColumnName("categoria")
            .HasMaxLength(100);

        builder.Property(c => c.DuracaoHoras)
            .HasColumnName("duracao_horas");

        builder.Property(c => c.Nivel)
            .HasColumnName("nivel")
            .HasMaxLength(50);

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("SYSDATE");
    }
}

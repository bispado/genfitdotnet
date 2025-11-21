using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenFit.Core.Entities;

namespace GenFit.Infrastructure.Data.Configurations;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.ToTable("SKILLS");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");

        builder.Property(s => s.Codigo)
            .HasColumnName("codigo")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(s => s.Nome)
            .HasColumnName("nome")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(s => s.Categoria)
            .HasColumnName("categoria")
            .HasMaxLength(100);

        builder.Property(s => s.Descricao)
            .HasColumnName("descricao")
            .HasMaxLength(500);

        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("SYSDATE");

        builder.HasIndex(s => s.Codigo).IsUnique();
    }
}

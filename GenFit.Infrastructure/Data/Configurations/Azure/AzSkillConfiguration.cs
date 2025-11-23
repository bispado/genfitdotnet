using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenFit.Core.Entities.Azure;

namespace GenFit.Infrastructure.Data.Configurations.Azure;

public class AzSkillConfiguration : IEntityTypeConfiguration<AzSkill>
{
    public void Configure(EntityTypeBuilder<AzSkill> builder)
    {
        builder.ToTable("AZ_SKILLS");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(s => s.Nome)
            .HasColumnName("nome")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(s => s.Categoria)
            .HasColumnName("categoria")
            .HasMaxLength(100);

        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
    }
}


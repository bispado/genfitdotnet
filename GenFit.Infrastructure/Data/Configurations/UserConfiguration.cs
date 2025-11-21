using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenFit.Core.Entities;

namespace GenFit.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("USERS");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id");

        builder.Property(u => u.Role)
            .HasColumnName("role")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(u => u.Nome)
            .HasColumnName("nome")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(u => u.SenhaHash)
            .HasColumnName("senha_hash")
            .HasMaxLength(255);

        builder.Property(u => u.Cpf)
            .HasColumnName("cpf")
            .HasMaxLength(14);

        builder.Property(u => u.Telefone)
            .HasColumnName("telefone")
            .HasMaxLength(20);

        builder.Property(u => u.DataNascimento)
            .HasColumnName("data_nascimento");

        builder.Property(u => u.LinkedInUrl)
            .HasColumnName("linkedin_url")
            .HasMaxLength(255);

        builder.Property(u => u.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("SYSDATE");

        builder.Property(u => u.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("SYSDATE");

        builder.HasIndex(u => u.Email).IsUnique();
    }
}

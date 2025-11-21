using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenFit.Core.Entities;

namespace GenFit.Infrastructure.Data.Configurations;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        // As colunas foram criadas em minúsculas no schema Oracle fornecido
        // No Oracle, quando criadas sem aspas, são convertidas para maiúsculas
        // Mas o schema mostra minúsculas, então provavelmente foram criadas com aspas
        // Tentando com minúsculas primeiro conforme o schema
        builder.ToTable("JOBS");

        builder.HasKey(j => j.Id);
        builder.Property(j => j.Id)
            .HasColumnName("ID")
            .HasColumnType("NUMBER(10)")
            .ValueGeneratedNever();

        builder.Property(j => j.Titulo)
            .HasColumnName("TITULO")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(j => j.Descricao)
            .HasColumnName("DESCRICAO")
            .HasColumnType("CLOB");

        builder.Property(j => j.Salario)
            .HasColumnName("SALARIO")
            .HasColumnType("NUMBER(10,2)");

        builder.Property(j => j.Localizacao)
            .HasColumnName("LOCALIZACAO")
            .HasMaxLength(150);

        builder.Property(j => j.TipoContrato)
            .HasColumnName("TIPO_CONTRATO")
            .HasMaxLength(50);

        builder.Property(j => j.Nivel)
            .HasColumnName("NIVEL")
            .HasMaxLength(50);

        builder.Property(j => j.ModeloTrabalho)
            .HasColumnName("MODELO_TRABALHO")
            .HasMaxLength(50);

        builder.Property(j => j.Departamento)
            .HasColumnName("DEPARTAMENTO")
            .HasMaxLength(100);

        builder.Property(j => j.CreatedAt)
            .HasColumnName("CREATED_AT")
            .HasDefaultValueSql("SYSDATE");

        builder.Property(j => j.UpdatedAt)
            .HasColumnName("UPDATED_AT")
            .HasDefaultValueSql("SYSDATE");
    }
}

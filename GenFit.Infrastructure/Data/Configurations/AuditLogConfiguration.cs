using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenFit.Core.Entities;

namespace GenFit.Infrastructure.Data.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AUDIT_LOGS");

        builder.HasKey(al => al.Id);
        builder.Property(al => al.Id).HasColumnName("id");

        builder.Property(al => al.TabelaAfetada)
            .HasColumnName("tabela_afetada")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(al => al.RegistroId)
            .HasColumnName("registro_id")
            .IsRequired();

        builder.Property(al => al.Operacao)
            .HasColumnName("operacao")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(al => al.UsuarioBanco)
            .HasColumnName("usuario_banco")
            .HasMaxLength(100);

        builder.Property(al => al.DataHora)
            .HasColumnName("data_hora")
            .HasDefaultValueSql("SYSDATE");

        builder.Property(al => al.DadosAntigos)
            .HasColumnName("dados_antigos")
            .HasColumnType("CLOB");

        builder.Property(al => al.DadosNovos)
            .HasColumnName("dados_novos")
            .HasColumnType("CLOB");
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenFit.Core.Entities;

namespace GenFit.Infrastructure.Data.Configurations;

public class QuestionnaireQuestionConfiguration : IEntityTypeConfiguration<QuestionnaireQuestion>
{
    public void Configure(EntityTypeBuilder<QuestionnaireQuestion> builder)
    {
        builder.ToTable("QUESTIONNAIRE_QUESTIONS");

        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id).HasColumnName("id");

        builder.Property(q => q.PerguntaTexto)
            .HasColumnName("pergunta_texto")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(q => q.Tipo)
            .HasColumnName("tipo")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(q => q.EscalaMin)
            .HasColumnName("escala_min");

        builder.Property(q => q.EscalaMax)
            .HasColumnName("escala_max");

        builder.Property(q => q.Opcoes)
            .HasColumnName("opcoes")
            .HasMaxLength(500);

        builder.Property(q => q.Categoria)
            .HasColumnName("categoria")
            .HasMaxLength(100);

        builder.Property(q => q.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("SYSDATE");
    }
}

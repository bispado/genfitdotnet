using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenFit.Core.Entities;

namespace GenFit.Infrastructure.Data.Configurations;

public class QuestionnaireAnswerConfiguration : IEntityTypeConfiguration<QuestionnaireAnswer>
{
    public void Configure(EntityTypeBuilder<QuestionnaireAnswer> builder)
    {
        builder.ToTable("QUESTIONNAIRE_ANSWERS");

        builder.HasKey(qa => qa.Id);
        builder.Property(qa => qa.Id).HasColumnName("id");

        builder.Property(qa => qa.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(qa => qa.QuestionId)
            .HasColumnName("question_id")
            .IsRequired();

        builder.Property(qa => qa.RespostaLikert)
            .HasColumnName("resposta_likert");

        builder.Property(qa => qa.RespostaTexto)
            .HasColumnName("resposta_texto")
            .HasMaxLength(1000);

        builder.Property(qa => qa.RespostaOpcao)
            .HasColumnName("resposta_opcao")
            .HasMaxLength(200);

        builder.Property(qa => qa.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("SYSDATE");

        builder.HasOne(qa => qa.User)
            .WithMany(u => u.QuestionnaireAnswers)
            .HasForeignKey(qa => qa.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(qa => qa.Question)
            .WithMany(q => q.QuestionnaireAnswers)
            .HasForeignKey(qa => qa.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

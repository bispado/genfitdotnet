using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenFit.Core.Entities;

namespace GenFit.Infrastructure.Data.Configurations;

public class CandidateSkillConfiguration : IEntityTypeConfiguration<CandidateSkill>
{
    public void Configure(EntityTypeBuilder<CandidateSkill> builder)
    {
        builder.ToTable("CANDIDATE_SKILLS");

        builder.HasKey(cs => cs.Id);
        builder.Property(cs => cs.Id).HasColumnName("id");

        builder.Property(cs => cs.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(cs => cs.SkillId)
            .HasColumnName("skill_id")
            .IsRequired();

        builder.Property(cs => cs.NivelProficiencia)
            .HasColumnName("nivel_proficiencia")
            .HasColumnType("NUMBER(3,2)");

        builder.Property(cs => cs.DataAquisicao)
            .HasColumnName("data_aquisicao")
            .HasDefaultValueSql("SYSDATE");

        builder.HasOne(cs => cs.User)
            .WithMany(u => u.CandidateSkills)
            .HasForeignKey(cs => cs.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cs => cs.Skill)
            .WithMany(s => s.CandidateSkills)
            .HasForeignKey(cs => cs.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(cs => new { cs.UserId, cs.SkillId }).IsUnique();
    }
}

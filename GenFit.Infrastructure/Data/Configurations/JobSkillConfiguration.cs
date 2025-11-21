using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GenFit.Core.Entities;

namespace GenFit.Infrastructure.Data.Configurations;

public class JobSkillConfiguration : IEntityTypeConfiguration<JobSkill>
{
    public void Configure(EntityTypeBuilder<JobSkill> builder)
    {
        builder.ToTable("JOB_SKILLS");

        builder.HasKey(js => js.Id);
        builder.Property(js => js.Id).HasColumnName("id");

        builder.Property(js => js.JobId)
            .HasColumnName("job_id")
            .IsRequired();

        builder.Property(js => js.SkillId)
            .HasColumnName("skill_id")
            .IsRequired();

        builder.Property(js => js.Obrigatoria)
            .HasColumnName("obrigatoria")
            .HasMaxLength(1)
            .HasDefaultValue("S");

        builder.Property(js => js.Peso)
            .HasColumnName("peso")
            .HasColumnType("NUMBER(3,2)")
            .HasDefaultValue(1.0m);

        builder.HasOne(js => js.Job)
            .WithMany(j => j.JobSkills)
            .HasForeignKey(js => js.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(js => js.Skill)
            .WithMany(s => s.JobSkills)
            .HasForeignKey(js => js.SkillId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

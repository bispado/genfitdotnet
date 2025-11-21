using Microsoft.EntityFrameworkCore;
using GenFit.Core.Entities;

namespace GenFit.Infrastructure.Data;

public class GenFitDbContext : DbContext
{
    public GenFitDbContext(DbContextOptions<GenFitDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CandidateSkill> CandidateSkills { get; set; }
    public DbSet<JobSkill> JobSkills { get; set; }
    public DbSet<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }
    public DbSet<QuestionnaireAnswer> QuestionnaireAnswers { get; set; }
    public DbSet<ModelResult> ModelResults { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas as configurações
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GenFitDbContext).Assembly);
    }
}

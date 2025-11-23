using Microsoft.EntityFrameworkCore;
using GenFit.Core.Entities.Azure;

namespace GenFit.Infrastructure.Data;

public class AzureSqlDbContext : DbContext
{
    public AzureSqlDbContext(DbContextOptions<AzureSqlDbContext> options) : base(options)
    {
    }

    public DbSet<AzJob> AzJobs { get; set; }
    public DbSet<AzCompany> AzCompanies { get; set; }
    public DbSet<AzSkill> AzSkills { get; set; }
    public DbSet<AzApplication> AzApplications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas as configurações do namespace Azure
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AzureSqlDbContext).Assembly,
            type => type.Namespace == "GenFit.Infrastructure.Data.Configurations.Azure");
    }
}


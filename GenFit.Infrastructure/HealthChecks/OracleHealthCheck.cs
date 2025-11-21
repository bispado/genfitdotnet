using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using GenFit.Infrastructure.Data;

namespace GenFit.Infrastructure.HealthChecks;

public class OracleHealthCheck : IHealthCheck
{
    private readonly GenFitDbContext _context;

    public OracleHealthCheck(GenFitDbContext context)
    {
        _context = context;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Testa a conexão com o Oracle executando uma query simples
            var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
            
            if (canConnect)
            {
                // Executa uma query simples para garantir que o banco está respondendo
                await _context.Database.ExecuteSqlRawAsync("SELECT 1 FROM DUAL", cancellationToken);
                return HealthCheckResult.Healthy("Oracle database is available");
            }
            
            return HealthCheckResult.Unhealthy("Oracle database is not available");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Oracle database check failed", ex);
        }
    }
}

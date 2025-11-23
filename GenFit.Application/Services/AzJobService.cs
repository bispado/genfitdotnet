using Microsoft.EntityFrameworkCore;
using GenFit.Application.Common;
using GenFit.Application.DTOs.Azure;
using GenFit.Core.Entities.Azure;
using GenFit.Infrastructure.Data;

namespace GenFit.Application.Services;

public class AzJobService : IAzJobService
{
    private readonly AzureSqlDbContext _context;

    public AzJobService(AzureSqlDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<AzJobDto>> GetAzJobsAsync(PaginationParameters parameters, string baseUrl)
    {
        var query = _context.AzJobs.AsQueryable();
        var totalCount = await query.CountAsync();

        var jobs = await query
            .OrderBy(j => j.Id)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .Select(j => new AzJobDto
            {
                Id = j.Id,
                Titulo = j.Titulo,
                Descricao = j.Descricao,
                Salario = j.Salario,
                Localizacao = j.Localizacao,
                CreatedAt = j.CreatedAt,
                UpdatedAt = j.UpdatedAt
            })
            .ToListAsync();

        var result = new PagedResult<AzJobDto>
        {
            Items = jobs,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            TotalCount = totalCount
        };

        // HATEOAS links
        var currentPath = $"{baseUrl}/api/v1/azjobs";
        result.Links["self"] = $"{currentPath}?pageNumber={parameters.PageNumber}&pageSize={parameters.PageSize}";
        result.Links["first"] = $"{currentPath}?pageNumber=1&pageSize={parameters.PageSize}";
        result.Links["last"] = $"{currentPath}?pageNumber={result.TotalPages}&pageSize={parameters.PageSize}";
        
        if (result.HasPrevious)
            result.Links["previous"] = $"{currentPath}?pageNumber={parameters.PageNumber - 1}&pageSize={parameters.PageSize}";
        
        if (result.HasNext)
            result.Links["next"] = $"{currentPath}?pageNumber={parameters.PageNumber + 1}&pageSize={parameters.PageSize}";

        return result;
    }

    public async Task<AzJobDto?> GetAzJobByIdAsync(int id)
    {
        try
        {
            var jobEntity = await _context.AzJobs
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Id == id);
            
            if (jobEntity == null)
                return null;

            return new AzJobDto
            {
                Id = jobEntity.Id,
                Titulo = jobEntity.Titulo,
                Descricao = jobEntity.Descricao,
                Salario = jobEntity.Salario,
                Localizacao = jobEntity.Localizacao,
                CreatedAt = jobEntity.CreatedAt,
                UpdatedAt = jobEntity.UpdatedAt
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao buscar job Azure com ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<AzJobDto> CreateAzJobAsync(CreateAzJobDto createDto)
    {
        try
        {
            var job = new AzJob
            {
                Titulo = createDto.Titulo,
                Descricao = createDto.Descricao,
                Salario = createDto.Salario,
                Localizacao = createDto.Localizacao,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.AzJobs.Add(job);
            await _context.SaveChangesAsync();

            return await GetAzJobByIdAsync(job.Id) ?? throw new Exception("Erro ao recuperar job criado");
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao criar job Azure: {ex.Message}", ex);
        }
    }

    public async Task<AzJobDto?> UpdateAzJobAsync(int id, UpdateAzJobDto updateDto)
    {
        try
        {
            var job = await _context.AzJobs
                .FirstOrDefaultAsync(j => j.Id == id);
            
            if (job == null)
                return null;

            if (!string.IsNullOrWhiteSpace(updateDto.Titulo))
                job.Titulo = updateDto.Titulo;
            
            if (updateDto.Descricao != null)
                job.Descricao = updateDto.Descricao;
            
            if (updateDto.Salario.HasValue)
                job.Salario = updateDto.Salario;
            
            if (updateDto.Localizacao != null)
                job.Localizacao = updateDto.Localizacao;

            job.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetAzJobByIdAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao atualizar job Azure com ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteAzJobAsync(int id)
    {
        try
        {
            var job = await _context.AzJobs
                .FirstOrDefaultAsync(j => j.Id == id);
            
            if (job == null)
                return false;

            _context.AzJobs.Remove(job);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao deletar job Azure com ID {id}: {ex.Message}", ex);
        }
    }
}


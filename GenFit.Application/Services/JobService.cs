using Microsoft.EntityFrameworkCore;
using GenFit.Application.Common;
using GenFit.Application.DTOs;
using GenFit.Core.Entities;
using GenFit.Infrastructure.Data;
using GenFit.Infrastructure.Services;

namespace GenFit.Application.Services;

public class JobService : IJobService
{
    private readonly GenFitDbContext _context;
    private readonly OracleProcedureService _procedureService;

    public JobService(GenFitDbContext context, OracleProcedureService procedureService)
    {
        _context = context;
        _procedureService = procedureService;
    }

    public async Task<PagedResult<JobDto>> GetJobsAsync(PaginationParameters parameters, string baseUrl)
    {
        var query = _context.Jobs.AsQueryable();
        var totalCount = await query.CountAsync();

        var jobs = await query
            .OrderBy(j => j.Id)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .Select(j => new JobDto
            {
                Id = j.Id,
                Titulo = j.Titulo,
                Descricao = j.Descricao,
                Salario = j.Salario,
                Localizacao = j.Localizacao,
                TipoContrato = j.TipoContrato,
                Nivel = j.Nivel,
                ModeloTrabalho = j.ModeloTrabalho,
                Departamento = j.Departamento,
                CreatedAt = j.CreatedAt,
                UpdatedAt = j.UpdatedAt
            })
            .ToListAsync();

        var result = new PagedResult<JobDto>
        {
            Items = jobs,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            TotalCount = totalCount
        };

        // HATEOAS links
        var currentPath = $"{baseUrl}/api/v1/jobs";
        result.Links["self"] = $"{currentPath}?pageNumber={parameters.PageNumber}&pageSize={parameters.PageSize}";
        result.Links["first"] = $"{currentPath}?pageNumber=1&pageSize={parameters.PageSize}";
        result.Links["last"] = $"{currentPath}?pageNumber={result.TotalPages}&pageSize={parameters.PageSize}";
        
        if (result.HasPrevious)
            result.Links["previous"] = $"{currentPath}?pageNumber={parameters.PageNumber - 1}&pageSize={parameters.PageSize}";
        
        if (result.HasNext)
            result.Links["next"] = $"{currentPath}?pageNumber={parameters.PageNumber + 1}&pageSize={parameters.PageSize}";

        return result;
    }

    public async Task<JobDto?> GetJobByIdAsync(int id)
    {
        try
        {
            // Buscar a entidade completa primeiro
            var jobEntity = await _context.Jobs
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Id == id);
            
            if (jobEntity == null)
                return null;

            // Mapear para DTO
            return new JobDto
            {
                Id = jobEntity.Id,
                Titulo = jobEntity.Titulo,
                Descricao = jobEntity.Descricao,
                Salario = jobEntity.Salario,
                Localizacao = jobEntity.Localizacao,
                TipoContrato = jobEntity.TipoContrato,
                Nivel = jobEntity.Nivel,
                ModeloTrabalho = jobEntity.ModeloTrabalho,
                Departamento = jobEntity.Departamento,
                CreatedAt = jobEntity.CreatedAt,
                UpdatedAt = jobEntity.UpdatedAt
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao buscar job com ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<JobDto> CreateJobAsync(CreateJobDto createDto)
    {
        // Usar procedure PRC_INSERT_JOB
        var jobId = await _procedureService.ExecutePrcInsertJobAsync(
            createDto.Titulo,
            createDto.Descricao,
            createDto.Salario,
            createDto.Localizacao,
            createDto.TipoContrato,
            createDto.Nivel,
            createDto.ModeloTrabalho,
            createDto.Departamento);

        var createdJob = await GetJobByIdAsync(jobId);
        
        return createdJob!;
    }

    public async Task<JobDto?> UpdateJobAsync(int id, UpdateJobDto updateDto)
    {
        try
        {
            var job = await _context.Jobs
                .Where(j => j.Id == id)
                .FirstOrDefaultAsync();
            
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
            
            if (updateDto.TipoContrato != null)
                job.TipoContrato = updateDto.TipoContrato;
            
            if (updateDto.Nivel != null)
                job.Nivel = updateDto.Nivel;
            
            if (updateDto.ModeloTrabalho != null)
                job.ModeloTrabalho = updateDto.ModeloTrabalho;
            
            if (updateDto.Departamento != null)
                job.Departamento = updateDto.Departamento;

            job.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetJobByIdAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao atualizar job com ID {id}: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteJobAsync(int id)
    {
        try
        {
            var job = await _context.Jobs
                .Where(j => j.Id == id)
                .FirstOrDefaultAsync();
            
            if (job == null)
                return false;

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao deletar job com ID {id}: {ex.Message}", ex);
        }
    }
}
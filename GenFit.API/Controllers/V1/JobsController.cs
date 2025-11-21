using Microsoft.AspNetCore.Mvc;
using GenFit.Application.Common;
using GenFit.Application.DTOs;
using GenFit.Application.Services;
using Asp.Versioning;
using System.Net;

namespace GenFit.API.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;
    private readonly ILogger<JobsController> _logger;

    public JobsController(IJobService jobService, ILogger<JobsController> logger)
    {
        _jobService = jobService;
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as vagas com paginação e HATEOAS
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<JobDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetJobs([FromQuery] PaginationParameters parameters)
    {
        try
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var result = await _jobService.GetJobsAsync(parameters, baseUrl);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar vagas");
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Obtém uma vaga por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(JobDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetJob(int id)
    {
        try
        {
            var job = await _jobService.GetJobByIdAsync(id);
            
            if (job == null)
                return NotFound(new { message = $"Vaga com ID {id} não encontrada" });
            
            return Ok(job);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar vaga {JobId}: {ErrorMessage}", id, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = $"Erro interno do servidor: {ex.Message}" });
        }
    }

    /// <summary>
    /// Cria uma nova vaga via procedure Oracle
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(JobDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var job = await _jobService.CreateJobAsync(createDto);
            
            return CreatedAtAction(nameof(GetJob), new { id = job.Id, version = "1.0" }, job);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar vaga");
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = $"Erro ao criar vaga: {ex.Message}" });
        }
    }

    /// <summary>
    /// Atualiza uma vaga existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(JobDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateJob(int id, [FromBody] UpdateJobDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var job = await _jobService.UpdateJobAsync(id, updateDto);
            
            if (job == null)
                return NotFound(new { message = $"Vaga com ID {id} não encontrada" });
            
            return Ok(job);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar vaga {JobId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Remove uma vaga
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> DeleteJob(int id)
    {
        try
        {
            var deleted = await _jobService.DeleteJobAsync(id);
            
            if (!deleted)
                return NotFound(new { message = $"Vaga com ID {id} não encontrada" });
            
            return Ok(new { message = "Deletado com sucesso", id = id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar vaga {JobId}: {ErrorMessage}", id, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = $"Erro interno do servidor: {ex.Message}" });
        }
    }
}

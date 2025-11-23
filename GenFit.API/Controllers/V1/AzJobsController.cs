using Microsoft.AspNetCore.Mvc;
using GenFit.Application.Common;
using GenFit.Application.DTOs.Azure;
using GenFit.Application.Services;
using Asp.Versioning;
using System.Net;

namespace GenFit.API.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AzJobsController : ControllerBase
{
    private readonly IAzJobService _azJobService;
    private readonly ILogger<AzJobsController> _logger;

    public AzJobsController(IAzJobService azJobService, ILogger<AzJobsController> logger)
    {
        _azJobService = azJobService;
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as vagas do Azure SQL com paginação e HATEOAS
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AzJobDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAzJobs([FromQuery] PaginationParameters parameters)
    {
        try
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var result = await _azJobService.GetAzJobsAsync(parameters, baseUrl);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar vagas Azure");
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Obtém uma vaga do Azure SQL por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AzJobDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetAzJob(int id)
    {
        try
        {
            var job = await _azJobService.GetAzJobByIdAsync(id);
            
            if (job == null)
                return NotFound(new { message = $"Vaga com ID {id} não encontrada" });
            
            return Ok(job);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar vaga Azure {JobId}: {ErrorMessage}", id, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = $"Erro interno do servidor: {ex.Message}" });
        }
    }

    /// <summary>
    /// Cria uma nova vaga no Azure SQL Database
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AzJobDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateAzJob([FromBody] CreateAzJobDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var job = await _azJobService.CreateAzJobAsync(createDto);
            
            return CreatedAtAction(nameof(GetAzJob), new { id = job.Id, version = "1.0" }, job);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar vaga Azure");
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = $"Erro ao criar vaga: {ex.Message}" });
        }
    }

    /// <summary>
    /// Atualiza uma vaga existente no Azure SQL Database
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(AzJobDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateAzJob(int id, [FromBody] UpdateAzJobDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var job = await _azJobService.UpdateAzJobAsync(id, updateDto);
            
            if (job == null)
                return NotFound(new { message = $"Vaga com ID {id} não encontrada" });
            
            return Ok(job);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar vaga Azure {JobId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Remove uma vaga do Azure SQL Database
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> DeleteAzJob(int id)
    {
        try
        {
            var deleted = await _azJobService.DeleteAzJobAsync(id);
            
            if (!deleted)
                return NotFound(new { message = $"Vaga com ID {id} não encontrada" });
            
            return Ok(new { message = "Deletado com sucesso", id = id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar vaga Azure {JobId}: {ErrorMessage}", id, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = $"Erro interno do servidor: {ex.Message}" });
        }
    }
}


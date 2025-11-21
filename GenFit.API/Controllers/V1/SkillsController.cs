using Microsoft.AspNetCore.Mvc;
using GenFit.Application.Common;
using GenFit.Application.DTOs;
using Asp.Versioning;
using System.Net;

namespace GenFit.API.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class SkillsController : ControllerBase
{
    private readonly ILogger<SkillsController> _logger;

    public SkillsController(ILogger<SkillsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as skills com paginação
    /// </summary>
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public IActionResult GetSkills([FromQuery] PaginationParameters parameters)
    {
        // TODO: Implementar service para Skills
        return Ok(new { message = "Endpoint em desenvolvimento" });
    }

    /// <summary>
    /// Obtém uma skill por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public IActionResult GetSkill(int id)
    {
        // TODO: Implementar service para Skills
        return Ok(new { message = "Endpoint em desenvolvimento" });
    }
}

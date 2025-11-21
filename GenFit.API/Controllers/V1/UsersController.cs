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
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os usuários com paginação e HATEOAS
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<UserDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetUsers([FromQuery] PaginationParameters parameters)
    {
        try
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var result = await _userService.GetUsersAsync(parameters, baseUrl);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar usuários");
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Obtém um usuário por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            
            if (user == null)
                return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
            
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuário {UserId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Cria um novo usuário via procedure Oracle
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.CreateUserAsync(createDto);
            
            return CreatedAtAction(nameof(GetUser), new { id = user.Id, version = "1.0" }, user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usuário");
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = $"Erro ao criar usuário: {ex.Message}" });
        }
    }

    /// <summary>
    /// Atualiza um usuário existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.UpdateUserAsync(id, updateDto);
            
            if (user == null)
                return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
            
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar usuário {UserId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Remove um usuário
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var deleted = await _userService.DeleteUserAsync(id);
            
            if (!deleted)
                return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar usuário {UserId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Erro interno do servidor" });
        }
    }
}

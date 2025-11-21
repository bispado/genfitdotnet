using Microsoft.EntityFrameworkCore;
using GenFit.Application.Common;
using GenFit.Application.DTOs;
using GenFit.Core.Entities;
using GenFit.Infrastructure.Data;
using GenFit.Infrastructure.Services;

namespace GenFit.Application.Services;

public class UserService : IUserService
{
    private readonly GenFitDbContext _context;
    private readonly OracleProcedureService _procedureService;

    public UserService(GenFitDbContext context, OracleProcedureService procedureService)
    {
        _context = context;
        _procedureService = procedureService;
    }

    public async Task<PagedResult<UserDto>> GetUsersAsync(PaginationParameters parameters, string baseUrl)
    {
        var query = _context.Users.AsQueryable();
        var totalCount = await query.CountAsync();

        var users = await query
            .OrderBy(u => u.Id)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Role = u.Role,
                Nome = u.Nome,
                Email = u.Email,
                Cpf = u.Cpf,
                Telefone = u.Telefone,
                DataNascimento = u.DataNascimento,
                LinkedInUrl = u.LinkedInUrl,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            })
            .ToListAsync();

        var result = new PagedResult<UserDto>
        {
            Items = users,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            TotalCount = totalCount
        };

        // HATEOAS links
        var currentPath = $"{baseUrl}/api/v1/users";
        result.Links["self"] = $"{currentPath}?pageNumber={parameters.PageNumber}&pageSize={parameters.PageSize}";
        result.Links["first"] = $"{currentPath}?pageNumber=1&pageSize={parameters.PageSize}";
        result.Links["last"] = $"{currentPath}?pageNumber={result.TotalPages}&pageSize={parameters.PageSize}";
        
        if (result.HasPrevious)
            result.Links["previous"] = $"{currentPath}?pageNumber={parameters.PageNumber - 1}&pageSize={parameters.PageSize}";
        
        if (result.HasNext)
            result.Links["next"] = $"{currentPath}?pageNumber={parameters.PageNumber + 1}&pageSize={parameters.PageSize}";

        return result;
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null)
            return null;

        return new UserDto
        {
            Id = user.Id,
            Role = user.Role,
            Nome = user.Nome,
            Email = user.Email,
            Cpf = user.Cpf,
            Telefone = user.Telefone,
            DataNascimento = user.DataNascimento,
            LinkedInUrl = user.LinkedInUrl,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createDto)
    {
        var userId = await _procedureService.ExecutePrcInsertUserAsync(
            createDto.Nome,
            createDto.Email,
            createDto.Role,
            createDto.SenhaHash,
            createDto.Cpf,
            createDto.Telefone,
            createDto.DataNascimento,
            createDto.LinkedInUrl);

        var createdUser = await GetUserByIdAsync(userId);
        
        return createdUser!;
    }

    public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateDto)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null)
            return null;

        if (!string.IsNullOrWhiteSpace(updateDto.Nome))
            user.Nome = updateDto.Nome;
        
        if (updateDto.Telefone != null)
            user.Telefone = updateDto.Telefone;
        
        if (updateDto.DataNascimento.HasValue)
            user.DataNascimento = updateDto.DataNascimento;
        
        if (updateDto.LinkedInUrl != null)
            user.LinkedInUrl = updateDto.LinkedInUrl;

        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetUserByIdAsync(id);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }
}

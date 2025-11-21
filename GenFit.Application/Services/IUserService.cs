using GenFit.Application.Common;
using GenFit.Application.DTOs;

namespace GenFit.Application.Services;

public interface IUserService
{
    Task<PagedResult<UserDto>> GetUsersAsync(PaginationParameters parameters, string baseUrl);
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto> CreateUserAsync(CreateUserDto createDto);
    Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateDto);
    Task<bool> DeleteUserAsync(int id);
}

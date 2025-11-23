using GenFit.Application.Common;
using GenFit.Application.DTOs.Azure;

namespace GenFit.Application.Services;

public interface IAzJobService
{
    Task<PagedResult<AzJobDto>> GetAzJobsAsync(PaginationParameters parameters, string baseUrl);
    Task<AzJobDto?> GetAzJobByIdAsync(int id);
    Task<AzJobDto> CreateAzJobAsync(CreateAzJobDto createDto);
    Task<AzJobDto?> UpdateAzJobAsync(int id, UpdateAzJobDto updateDto);
    Task<bool> DeleteAzJobAsync(int id);
}


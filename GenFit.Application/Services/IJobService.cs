using GenFit.Application.Common;
using GenFit.Application.DTOs;

namespace GenFit.Application.Services;

public interface IJobService
{
    Task<PagedResult<JobDto>> GetJobsAsync(PaginationParameters parameters, string baseUrl);
    Task<JobDto?> GetJobByIdAsync(int id);
    Task<JobDto> CreateJobAsync(CreateJobDto createDto);
    Task<JobDto?> UpdateJobAsync(int id, UpdateJobDto updateDto);
    Task<bool> DeleteJobAsync(int id);
}

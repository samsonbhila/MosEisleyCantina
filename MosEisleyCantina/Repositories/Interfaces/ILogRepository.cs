using MosEisleyCantina.Models.DTOs;
using MosEisleyCantina.Models.Entities;

namespace MosEisleyCantina.Repositories.Interfaces
{
    public interface ILogRepository
    {
        Task<List<LogEntity>> GetLogsAsync(LogSearchRequest request);
    }
}

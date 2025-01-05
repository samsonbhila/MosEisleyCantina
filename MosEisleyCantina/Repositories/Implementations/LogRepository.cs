using Microsoft.EntityFrameworkCore;
using MosEisleyCantina.Models.DTOs;
using MosEisleyCantina.Models.Entities;
using MosEisleyCantinaAPI.Data;
using Serilog;

namespace MosEisleyCantina.Repositories.Implementations
{
    public class LogRepository
    {
        private readonly AppDbContext _context;

        public LogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<LogEntity>> GetLogsAsync(LogSearchRequest request)
        {
            var query = _context.Logs.AsQueryable();

            if (request.StartDate.HasValue)
            {
                query = query.Where(log => log.TimeStamp >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(log => log.TimeStamp <= request.EndDate.Value);
            }

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(log => log.Message.Contains(request.SearchTerm) || log.Exception.Contains(request.SearchTerm));
            }

            if (!string.IsNullOrEmpty(request.Level))
            {
                query = query.Where(log => log.Level == request.Level);
            }

            return await query.ToListAsync();
        }
    }

}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MosEisleyCantina.Models.DTOs;
using MosEisleyCantina.Repositories.Implementations;
using MosEisleyCantinaAPI.Data;

namespace MosEisleyCantina.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly LogRepository _logRepository;
        private readonly AppDbContext _context;
        private readonly IMemoryCache _memoryCache;  

        public LogsController(LogRepository logRepository, AppDbContext context, IMemoryCache memoryCache)
        {
            _logRepository = logRepository;
            _context = context;
            _memoryCache = memoryCache;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllLogs(int page = 1, int pageSize = 100)
        {
            try
            {
                if (page < 1 || pageSize < 1)
                {
                    return BadRequest("Page and PageSize must be greater than 0.");
                }

                string cacheKey = $"logs-page-{page}-size-{pageSize}";
                if (_memoryCache.TryGetValue(cacheKey, out var cachedLogs))
                {
                    return Ok(cachedLogs); 
                }

                var totalLogs = await _context.Logs.CountAsync();
                var logs = await _context.Logs
                                         .Skip((page - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();

                if (!logs.Any())
                {
                    return NotFound("No logs found.");
                }

                var result = new
                {
                    TotalLogs = totalLogs,
                    Page = page,
                    PageSize = pageSize,
                    Logs = logs
                };

                _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(5));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetLogs([FromQuery] LogSearchRequest request)
        {
            if (request.StartDate.HasValue && !request.EndDate.HasValue)
            {
                return BadRequest("EndDate is required if StartDate is provided.");
            }

            if (request.EndDate.HasValue && !request.StartDate.HasValue)
            {
                return BadRequest("StartDate is required if EndDate is provided.");
            }

            var logs = await _logRepository.GetLogsAsync(request);

            if (logs == null || !logs.Any())
            {
                return NotFound("No logs found for the specified parameters.");
            }

            return Ok(logs);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("time-frame")]
        public async Task<IActionResult> GetLogsByTimeFrame([FromQuery] string timeFrame, [FromQuery] string searchTerm)
        {
            DateTime? startDate = null;
            DateTime? endDate = DateTime.UtcNow;

            switch (timeFrame.ToLower())
            {
                case "day":
                    startDate = endDate.Value.Date;
                    break;

                case "week":
                    startDate = endDate.Value.AddDays(-7);
                    break;

                case "month":
                    startDate = endDate.Value.AddMonths(-1);
                    break;

                case "year":
                    startDate = endDate.Value.AddYears(-1);
                    break;

                default:
                    return BadRequest("Invalid time frame specified. Valid values are: day, week, month, year.");
            }

            var request = new LogSearchRequest
            {
                StartDate = startDate,
                EndDate = endDate,
                SearchTerm = searchTerm
            };

            var logs = await _logRepository.GetLogsAsync(request);

            if (logs == null || !logs.Any())
            {
                return NotFound("No logs found for the specified time frame.");
            }

            return Ok(logs);
        }
    }
}
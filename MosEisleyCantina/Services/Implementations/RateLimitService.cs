//using AspNetCoreRateLimit;
//using Microsoft.Extensions.Caching.Memory;
//using Microsoft.Extensions.Options;
//using MosEisleyCantina.Configurations;
//using MosEisleyCantina.Services.Interfaces;
//using System.Collections.Concurrent;
//using System.Net;
//using System.Threading.Tasks;

//namespace MosEisleyCantina.Services.Implementations
//{
//    public class RateLimitService : IRateLimitService
//    {
//        private readonly IpRateLimitProcessor _rateLimitProcessor;
//        private readonly IMemoryCache _memoryCache;
//        private readonly IpRateLimitOptions _options;

//        public RateLimitService(IpRateLimitProcessor rateLimitProcessor, IMemoryCache memoryCache, IpRateLimitOptions options)
//        {
//            _rateLimitProcessor = rateLimitProcessor;
//            _memoryCache = memoryCache;
//            _options = options;
//        }

//        public async Task<bool> IsRateLimitedAsync(string clientId)
//        {
//            
//            var isRateLimited = await _rateLimitProcessor.IsClientRateLimitedAsync(clientId);
//            return isRateLimited;
//        }
//    }
//}

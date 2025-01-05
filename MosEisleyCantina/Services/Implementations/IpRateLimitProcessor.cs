//using AspNetCoreRateLimit;

//namespace MosEisleyCantina.Services.Implementations
//{
//    public class IpRateLimitProcessor
//    {
//        private readonly IIpPolicyStore _policyStore;
//        private readonly IRateLimitCounterStore _counterStore;
//        private readonly IProcessingStrategy _processingStrategy;
//        private readonly IpRateLimitOptions _options;

//        public IpRateLimitProcessor(IIpPolicyStore policyStore, IRateLimitCounterStore counterStore, IProcessingStrategy processingStrategy, IpRateLimitOptions options)
//        {
//            _policyStore = policyStore;
//            _counterStore = counterStore;
//            _processingStrategy = processingStrategy;
//            _options = options;
//        }

//        public async Task<bool> IsClientRateLimitedAsync(string clientId)
//        {
//            // Check if the client is rate-limited based on your rate-limiting logic
//            var policy = await _policyStore.GetAsync(clientId);
//            if (policy == null)
//            {
//                return false; // No rate-limiting policy found for this client
//            }

//            var counter = await _counterStore.GetAsync(clientId);
//            if (counter == null || counter.Count < policy.Limit)
//            {
//                return false; // Client hasn't exceeded the rate limit
//            }

//            return true; // Client has exceeded the rate limit
//        }
//    }
//}

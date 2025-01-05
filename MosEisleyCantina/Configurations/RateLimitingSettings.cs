namespace MosEisleyCantina.Configurations
{
    public class RateLimitingSettings
    {
        public bool EnableEndpointRateLimiting { get; set; }
        public bool StackBlockedRequests { get; set; }
        public string RealIpHeader { get; set; }
        public string ClientIdHeader { get; set; }
        public List<RateLimitRule> GeneralRules { get; set; }
    }

    public class RateLimitRule
    {
        public string Endpoint { get; set; }
        public string Period { get; set; }
        public int Limit { get; set; }
    }

}

using Hangfire.Dashboard;

namespace MosEisleyCantina.Configurations
{
    public class HangfireDashboardAuthorization : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {

            return true;
        }
    }
}

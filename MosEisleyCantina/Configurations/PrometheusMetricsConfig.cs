using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

public static class PrometheusMetricsConfig
{
    public static void AddPrometheusMetrics(this IServiceCollection services)
    {
        services.AddControllers(); 
        services.AddMetrics(); 
    }

    public static void UsePrometheusMetrics(this IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseMetricServer(); 
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapMetrics();   
        });
    }
}

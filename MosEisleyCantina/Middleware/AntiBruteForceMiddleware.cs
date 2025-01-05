using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace MosEisleyCantina.Middleware
{
    public class AntiBruteForceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AntiBruteForceMiddleware> _logger;

        public AntiBruteForceMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory, ILogger<AntiBruteForceMiddleware> logger)
        {
            _next = next;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/login"))
            {
                if (context.Request.ContentType != "application/json")
                {
                    context.Response.StatusCode = 415; 
                    await context.Response.WriteAsync("Content-Type must be application/json.");
                    return;
                }

                var requestBody = await new System.IO.StreamReader(context.Request.Body).ReadToEndAsync();

                try
                {
                    var jsonDoc = JsonDocument.Parse(requestBody);
                    var userId = jsonDoc.RootElement.GetProperty("user_id").GetString();
                    var deviceId = jsonDoc.RootElement.GetProperty("device_id").GetString();

                    if (!string.IsNullOrEmpty(userId))
                        _logger.LogInformation($"User ID: {userId}");
                    else
                        _logger.LogWarning("User ID is missing!");

                    if (!string.IsNullOrEmpty(deviceId))
                        _logger.LogInformation($"Device ID: {deviceId}");
                    else
                        _logger.LogWarning("Device ID is missing!");

                    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(deviceId))
                    {
                        context.Response.StatusCode = 400; 
                        await context.Response.WriteAsync("Missing user ID or device ID.");
                        return;
                    }

                    var client = _httpClientFactory.CreateClient();
                    var payload = new
                    {
                        user_id = userId,
                        device_id = deviceId
                    };

                    var jsonPayload = JsonSerializer.Serialize(payload);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("http://localhost:8000/verify-device", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        context.Response.StatusCode = 403; 
                        await context.Response.WriteAsync("Access Denied: This is not your registered device.");
                        return;
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Error parsing JSON request.");
                    context.Response.StatusCode = 400; 
                    await context.Response.WriteAsync("Invalid JSON format.");
                    return;
                }
            }

            await _next(context);
        }
    }
}

//using System.Threading.RateLimiting;

//namespace MosEisleyCantina.Middleware
//{
//    public class RateLimitMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public RateLimitMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            // Check if rate limiting has been exceeded
//            var rateLimiter = context.RequestServices.GetRequiredService<IRateLimiter>();
//            var result = await rateLimiter.CheckAsync(context);

//            if (!result.IsAllowed)
//            {
//                // Set the status code for rate limiting exceeded
//                context.Response.StatusCode = 429; // Too Many Requests

//                // Create the response body with your custom message
//                var responseMessage = new
//                {
//                    error = "Too Many Requests",
//                    message = "You have reached your limit for today. Please try again later."
//                };

//                // Set the content type to JSON and write the response body
//                context.Response.ContentType = "application/json";
//                await context.Response.WriteAsJsonAsync(responseMessage);

//                return; // Prevent further processing of the request
//            }

//            // Allow the request to proceed if not rate-limited
//            await _next(context);
//        }
//    }

//}

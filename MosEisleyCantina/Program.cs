using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MosEisleyCantina.Middleware;
using MosEisleyCantinaAPI.Data;
using MosEisleyCantinaAPI.Interfaces;
using MosEisleyCantinaAPI.Services;
using System.Threading.RateLimiting;
using System.Text;
using Hangfire;
using Prometheus;
using Hangfire.SqlServer;
using MosEisleyCantina.Jobs;
using MosEisleyCantinaAPI.Services.Implementations;
using MosEisleyCantinaAPI.Services.Interfaces;
using MosEisleyCantinaAPI.Repositories.Implementations;
using MosEisleyCantinaAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using MosEisleyCantina.Repositories.Implementations;
using MosEisleyCantina.Repositories.Interfaces;
using Hangfire.Dashboard;
using MosEisleyCantina.Configurations;
using System.Data;
using Serilog.Debugging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configure Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
// Check and create the database
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Serilog.Debugging.SelfLog.Enable(msg => File.AppendAllText("serilog-self-log.txt", msg));

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))
    {
        AutoRegisterTemplate = false,
        IndexFormat = "logs-{0:yyyy.MM.dd}"
    })
    .WriteTo.MSSqlServer(
        connectionString,
        sinkOptions: new MSSqlServerSinkOptions()
        {
            TableName = "Logs",
            AutoCreateSqlTable = true
        },
        columnOptions: new ColumnOptions()
        {
            AdditionalColumns = new List<SqlColumn>
            {
                // new SqlColumn { ColumnName = "Properties", DataType = SqlDbType.NVarChar, DataLength = -1 },
                //new SqlColumn { ColumnName = "Exception", DataType = SqlDbType.NVarChar, DataLength = -1 }
            }
        })
    .CreateLogger();

// Add Serilog for logging
builder.Host.UseSerilog();

// Hangfire server
builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseDefaultTypeSerializer()
          .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configure Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
})
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Google:client_id"];
    googleOptions.ClientSecret = builder.Configuration["Google:client_secret"];
    googleOptions.CallbackPath = "/signin-google";
})
.AddCookie(options =>
{
    options.Events.OnRedirectToAccessDenied = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);
        }
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);
        }
        return Task.CompletedTask;
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.WithOrigins(
            "http://localhost:3000",
            "http://localhost:8080/swagger/index.html",
            "http://localhost:5087/swagger/index.html"
        )
        .AllowAnyMethod()
        .AllowAnyHeader());
});


// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // JWT Bearer authentication definition
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer' [space] and then your valid JWT token in the text input below.\r\n\r\nExample: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // Add Google OAuth2 definition
    options.AddSecurityDefinition("GoogleOAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
                TokenUrl = new Uri("https://oauth2.googleapis.com/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "email", "Access your email address" },
                    { "profile", "Access your profile information" }
                }
            }
        },
        Description = "OAuth2 Authentication using Google"
    });

    // JWT security requirement globally
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Google OAuth2 security requirement globally
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "GoogleOAuth"
                }
            },
            new[] { "email", "profile" }
        }
    });
});


// response compression service
builder.Services.AddResponseCompression(options =>
{
    // compression for HTTPS
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});


builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromSeconds(5),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));

    options.OnRejected = async (context, cancellationToken) =>
    {
        var httpContext = context.HttpContext;
        Console.WriteLine("Rate limit exceeded globally for IP: {0}", httpContext.Connection.RemoteIpAddress);

        httpContext.Response.StatusCode = 429; // Too Many Requests
        httpContext.Response.ContentType = "application/json";

        var responseMessage = new
        {
            error = "Too Many Requests",
            message = "You have exceeded your rate limit. Please try again later."
        };

        await httpContext.Response.WriteAsJsonAsync(responseMessage, cancellationToken);
    };
});



builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                      .AddEnvironmentVariables();
builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddScoped<LogRepository>();
builder.Services.AddScoped<DBInitialization>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IDrinkService, DrinkService>();
builder.Services.AddScoped<IDishRepository, DishRepository>();
builder.Services.AddScoped<IDrinkRepository, DrinkRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<RatingStatsJob>();
builder.Services.AddHangfireServer();
builder.Services.AddHttpClient();
builder.Services.AddPrometheusMetrics();
builder.Services.AddMemoryCache();



var app = builder.Build();


// Initialize the database at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var dbInitialization = services.GetRequiredService<DBInitialization>();
        await dbInitialization.InitializeAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing the database.");
    }
}

////data seeding 
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await SeedData.Initialize(scope.ServiceProvider, context);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
       c =>
       {
           c.SwaggerEndpoint("/swagger/v1/swagger.json", "MosEisleyCantina V1");

           // OAuth2 settings for Swagger UI

           c.OAuthClientId("Google:client_id");
           c.OAuthClientSecret("Google:client_secret");
           c.OAuthAppName("MosEisleyCantina - Swagger");
           c.OAuthScopeSeparator(" ");
           c.OAuthUsePkce(); // PKCE for enhanced security
           c.OAuthScopes("email profile");
       });
}

app.UseHttpsRedirection();

app.UseMiddleware<AntiBruteForceMiddleware>();

app.UseRateLimiter();


app.UseRouting();

app.UseCors("AllowAll");


app.UseAuthentication();
app.UseAuthorization();

app.UseHttpMetrics();

app.UsePrometheusMetrics();

app.UseSerilogRequestLogging();

app.UseResponseCompression();


app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireDashboardAuthorization() }
});

RecurringJob.AddOrUpdate<RatingStatsJob>(
    "calculate-top-rated-items",
    job => job.CalculateTopRatedItemsAsync(),
    Cron.Daily);

app.MapControllers();

app.Run();
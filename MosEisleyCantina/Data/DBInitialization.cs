using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using MosEisleyCantinaAPI.Data;
using MosEisleyCantinaAPI.Models;

namespace MosEisleyCantinaAPI.Data
{
    public class DBInitialization
    {
        private readonly ILogger<DBInitialization> _logger;

        public DBInitialization(ILogger<DBInitialization> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var dbContext = services.GetRequiredService<AppDbContext>();

                await dbContext.Database.MigrateAsync();

                await SeedData(dbContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during database initialization.");

                throw new DatabaseInitializationException("An error occurred during database initialization.", ex);
            }
        }

        private static async Task SeedData(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        UserName = "admin@example.com",
                        Email = "admin@example.com",
                        PasswordHash = "P@ssword123", 
                        FirstName = "Adam",
                        LastName = "Dawg"
                    },
                    new User
                    {
                        UserName = "user@example.com",
                        Email = "user@example.com",
                        PasswordHash = "P@ssword123", 
                        FirstName = "Excel",
                        LastName = "Ming"
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }

    public class DatabaseInitializationException : Exception
    {
        public DatabaseInitializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

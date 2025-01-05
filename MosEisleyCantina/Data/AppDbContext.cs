using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MosEisleyCantina.Models.Entities;
using MosEisleyCantinaAPI.Models;
using MosEisleyCantinaAPI.Models.Entities;

namespace MosEisleyCantinaAPI.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public new DbSet<User> Users { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<LogEntity> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Dish>()
                .Property(d => d.Price)
                .HasPrecision(18, 2);  

            builder.Entity<Drink>()
                .Property(d => d.Price)
                .HasPrecision(18, 2);  
        }
    }
}

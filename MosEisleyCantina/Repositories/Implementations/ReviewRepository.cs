using Microsoft.EntityFrameworkCore;
using MosEisleyCantinaAPI.Data;
using MosEisleyCantinaAPI.Models.Entities;
using MosEisleyCantinaAPI.Repositories.Interfaces;

namespace MosEisleyCantinaAPI.Repositories.Implementations
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews
                .Include(r => r.User) 
                .ToListAsync();
        }

        public async Task<Review> GetReviewByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.User) 
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

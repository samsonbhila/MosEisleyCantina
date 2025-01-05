using MosEisleyCantinaAPI.Models.Entities;

namespace MosEisleyCantinaAPI.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review> GetReviewByIdAsync(int id);
        Task AddReviewAsync(Review review);
        Task<bool> SaveChangesAsync();

    }
}

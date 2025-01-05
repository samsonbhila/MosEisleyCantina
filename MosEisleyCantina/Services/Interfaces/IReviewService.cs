using MosEisleyCantinaAPI.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MosEisleyCantinaAPI.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDTO>> GetAllReviewsAsync();
        Task<ReviewDTO> GetReviewByIdAsync(int id);
        Task<bool> AddReviewAsync(string content, int rating, string userId);
    }
}

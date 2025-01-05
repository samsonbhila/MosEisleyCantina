using AutoMapper;
using MosEisleyCantinaAPI.Models.DTOs;
using MosEisleyCantinaAPI.Models.Entities;
using MosEisleyCantinaAPI.Repositories.Interfaces;
using MosEisleyCantinaAPI.Services.Interfaces;

namespace MosEisleyCantinaAPI.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<ReviewDTO>> GetAllReviewsAsync()
        {
            var reviews = await _reviewRepository.GetAllReviewsAsync();

            return reviews.Select(r => new ReviewDTO
            {
                Id = r.Id,
                Content = r.Content,
                Rating = r.Rating,
                UserName = r.User.UserName, 
                CreatedAt = r.CreatedAt
            });
        }

        public async Task<ReviewDTO> GetReviewByIdAsync(int id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null) return null;

            return new ReviewDTO
            {
                Id = review.Id,
                Content = review.Content,
                Rating = review.Rating,
                UserName = review.User.UserName,
                CreatedAt = review.CreatedAt
            };
        }

        public async Task<bool> AddReviewAsync(string content, int rating, string userId)
        {
            var review = new Review
            {
                Content = content,
                Rating = rating,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.AddReviewAsync(review);
            return await _reviewRepository.SaveChangesAsync();
        }
    }
}

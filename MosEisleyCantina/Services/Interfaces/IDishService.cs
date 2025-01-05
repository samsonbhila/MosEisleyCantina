using MosEisleyCantina.Models.DTOs;
using MosEisleyCantinaAPI.Models.DTOs;

namespace MosEisleyCantinaAPI.Services.Interfaces
{
    public interface IDishService
    {
        Task<IEnumerable<DishDTO>> GetAllAsync();
        Task<DishDTO> GetByIdAsync(int id);
        Task AddAsync(DishDTO dish);
        Task UpdateAsync(int id, DishDTO dish);
        Task DeleteAsync(int id);
        Task<IEnumerable<DishDTO>> SearchAsync(string query);
        Task<bool> AddRatingAsync(int dishId, double rating);
        Task<ItemRatingResponseDTO> GetRatingAsync(int dishId);
    }

}

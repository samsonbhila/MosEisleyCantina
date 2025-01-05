using MosEisleyCantina.Models.DTOs;
using MosEisleyCantinaAPI.Models.DTOs;

namespace MosEisleyCantinaAPI.Services.Interfaces
{
    public interface IDrinkService
    {
        Task<IEnumerable<DrinkDTO>> GetAllAsync();
        Task<DrinkDTO> GetByIdAsync(int id);
        Task AddAsync(DrinkDTO drink);
        Task UpdateAsync(int id, DrinkDTO drink);
        Task DeleteAsync(int id);
        Task<IEnumerable<DrinkDTO>> SearchAsync(string query);
        Task<bool> AddRatingAsync(int drinkId, double rating);
        Task<ItemRatingResponseDTO> GetRatingAsync(int drinkId);
    }

}

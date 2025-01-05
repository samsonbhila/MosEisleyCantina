using MosEisleyCantinaAPI.Models.DTOs;
using MosEisleyCantinaAPI.Models.Entities;

namespace MosEisleyCantinaAPI.Repositories.Interfaces
{
    public interface IDishRepository
    {
        Task<IEnumerable<Dish>> GetAllAsync();
        Task<Dish> GetByIdAsync(int id);
        Task AddAsync(Dish dish);
        Task UpdateAsync(Dish dish);
        Task DeleteAsync(int id);
        Task<IEnumerable<Dish>> SearchAsync(string query);
        Task<IEnumerable<Dish>> GetTopRatedAsync(int count); 
    }

}

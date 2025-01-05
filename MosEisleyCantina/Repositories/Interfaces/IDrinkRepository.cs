using MosEisleyCantinaAPI.Models.Entities;

namespace MosEisleyCantinaAPI.Repositories.Interfaces
{
    public interface IDrinkRepository
    {
        Task<IEnumerable<Drink>> GetAllAsync();
        Task<Drink> GetByIdAsync(int id);
        Task AddAsync(Drink drink);
        Task UpdateAsync(Drink drink);
        Task DeleteAsync(int id);
        Task<IEnumerable<Drink>> SearchAsync(string query);
        Task<IEnumerable<Drink>> GetTopRatedAsync(int count);
    }

}

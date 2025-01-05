using Microsoft.EntityFrameworkCore;
using MosEisleyCantinaAPI.Data;
using MosEisleyCantinaAPI.Models.Entities;
using MosEisleyCantinaAPI.Repositories.Interfaces;

namespace MosEisleyCantinaAPI.Repositories.Implementations
{
    public class DishRepository : IDishRepository
    {
        private readonly AppDbContext _context;

        public DishRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dish>> GetAllAsync()
        {
            return await _context.Dishes.ToListAsync();
        }

        public async Task<Dish> GetByIdAsync(int id)
        {
            return await _context.Dishes.FindAsync(id);
        }

        public async Task AddAsync(Dish dish)
        {
            await _context.Dishes.AddAsync(dish);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Dish dish)
        {
            _context.Dishes.Update(dish);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dish = await GetByIdAsync(id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Dish>> GetTopRatedAsync(int count)
        {
            return await _context.Dishes
                .OrderByDescending(d => d.Rating)
                .ThenByDescending(d => d.RatingCount) 
                .Take(count)
                .ToListAsync();
        }


        public async Task<IEnumerable<Dish>> SearchAsync(string query)
        {
            return await _context.Dishes
                .Where(d => d.Name.Contains(query) || d.Description.Contains(query))
                .ToListAsync();
        }
    }

}

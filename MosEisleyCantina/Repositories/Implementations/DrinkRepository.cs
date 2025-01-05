using Microsoft.EntityFrameworkCore;
using MosEisleyCantinaAPI.Data;
using MosEisleyCantinaAPI.Models.Entities;
using MosEisleyCantinaAPI.Repositories.Interfaces;

namespace MosEisleyCantinaAPI.Repositories.Implementations
{
    public class DrinkRepository : IDrinkRepository
    {
        private readonly AppDbContext _context;

        public DrinkRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Drink>> GetAllAsync()
        {
            return await _context.Drinks.ToListAsync();
        }

        public async Task<Drink> GetByIdAsync(int id)
        {
            return await _context.Drinks.FindAsync(id);
        }

        public async Task AddAsync(Drink drink)
        {
            await _context.Drinks.AddAsync(drink);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Drink drink)
        {
            _context.Drinks.Update(drink);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var drink = await GetByIdAsync(id);
            if (drink != null)
            {
                _context.Drinks.Remove(drink);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Drink>> GetTopRatedAsync(int count)
        {
            return await _context.Drinks
                .OrderByDescending(d => d.Rating)
                .ThenByDescending(d => d.RatingCount) 
                .Take(count)
                .ToListAsync();
        }


        public async Task<IEnumerable<Drink>> SearchAsync(string query)
        {
            return await _context.Drinks
                .Where(d => d.Name.Contains(query) || d.Description.Contains(query))
                .ToListAsync();
        }
    }

}

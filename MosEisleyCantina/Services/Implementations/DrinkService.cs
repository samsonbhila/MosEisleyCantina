using AutoMapper;
using MosEisleyCantina.Models.DTOs;
using MosEisleyCantinaAPI.Models.DTOs;
using MosEisleyCantinaAPI.Models.Entities;
using MosEisleyCantinaAPI.Repositories.Interfaces;
using MosEisleyCantinaAPI.Services.Interfaces;


namespace MosEisleyCantinaAPI.Services.Implementations
{
    public class DrinkService : IDrinkService
    {
        private readonly IDrinkRepository _drinkRepository;

        public DrinkService(IDrinkRepository drinkRepository)
        {
            _drinkRepository = drinkRepository;
        }

        public async Task<IEnumerable<DrinkDTO>> GetAllAsync()
        {
            var drinks = await _drinkRepository.GetAllAsync();
            return drinks.Select(d => new DrinkDTO
            {
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                ImageUrl = d.ImageUrl
            });
        }

        public async Task<DrinkDTO> GetByIdAsync(int id)
        {
            var drink = await _drinkRepository.GetByIdAsync(id);
            return new DrinkDTO
            {
                Name = drink.Name,
                Description = drink.Description,
                Price = drink.Price,
                ImageUrl = drink.ImageUrl
            };
        }

        public async Task AddAsync(DrinkDTO drink)
        {
            var newDrink = new Drink
            {
                Name = drink.Name,
                Description = drink.Description,
                Price = drink.Price,
                ImageUrl = drink.ImageUrl
            };
            await _drinkRepository.AddAsync(newDrink);
        }

        public async Task UpdateAsync(int id, DrinkDTO drink)
        {
            var existingDrink = await _drinkRepository.GetByIdAsync(id);
            if (existingDrink != null)
            {
                existingDrink.Name = drink.Name;
                existingDrink.Description = drink.Description;
                existingDrink.Price = drink.Price;
                existingDrink.ImageUrl = drink.ImageUrl;

                await _drinkRepository.UpdateAsync(existingDrink);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _drinkRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<DrinkDTO>> SearchAsync(string query)
        {
            var drinks = await _drinkRepository.SearchAsync(query);
            return drinks.Select(d => new DrinkDTO
            {
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                ImageUrl = d.ImageUrl
            });
        }

        public async Task<bool> AddRatingAsync(int drinkId, double rating)
        {
            var drink = await _drinkRepository.GetByIdAsync(drinkId);
            if (drink == null) return false;

            drink.RatingCount++;
            drink.Rating = (drink.Rating * (drink.RatingCount - 1) + rating) / drink.RatingCount;

            await _drinkRepository.UpdateAsync(drink);
            return true;
        }

        public async Task<ItemRatingResponseDTO> GetRatingAsync(int drinkId)
        {
            var drink = await _drinkRepository.GetByIdAsync(drinkId);
            if (drink == null) return null;

            return new ItemRatingResponseDTO
            {
                Id = drink.Id,
                Name = drink.Name,
                AverageRating = drink.Rating,
                RatingCount = drink.RatingCount
            };
        }
    }
}



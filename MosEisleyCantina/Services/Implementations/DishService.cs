using AutoMapper;
using MosEisleyCantina.Models.DTOs;
using MosEisleyCantinaAPI.Models.DTOs;
using MosEisleyCantinaAPI.Models.Entities;
using MosEisleyCantinaAPI.Repositories.Interfaces;
using MosEisleyCantinaAPI.Services.Interfaces;


namespace MosEisleyCantinaAPI.Services.Implementations
{
    public class DishService : IDishService
    {
        private readonly IDishRepository _dishRepository;

        public DishService(IDishRepository dishRepository)
        {
            _dishRepository = dishRepository;
        }

        public async Task<IEnumerable<DishDTO>> GetAllAsync()
        {
            var dishes = await _dishRepository.GetAllAsync();
            return dishes.Select(d => new DishDTO
            {
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                ImageUrl = d.ImageUrl
            });
        }

        public async Task<DishDTO> GetByIdAsync(int id)
        {
            var dish = await _dishRepository.GetByIdAsync(id);
            return new DishDTO
            {
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                ImageUrl = dish.ImageUrl
            };
        }

        public async Task AddAsync(DishDTO dish)
        {
            var newDish = new Dish
            {
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                ImageUrl = dish.ImageUrl
            };
            await _dishRepository.AddAsync(newDish);
        }

        public async Task UpdateAsync(int id, DishDTO dish)
        {
            var existingDish = await _dishRepository.GetByIdAsync(id);
            if (existingDish != null)
            {
                existingDish.Name = dish.Name;
                existingDish.Description = dish.Description;
                existingDish.Price = dish.Price;
                existingDish.ImageUrl = dish.ImageUrl;

                await _dishRepository.UpdateAsync(existingDish);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _dishRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<DishDTO>> SearchAsync(string query)
        {
            var dishes = await _dishRepository.SearchAsync(query);
            return dishes.Select(d => new DishDTO
            {
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                ImageUrl = d.ImageUrl
            });
        }

        public async Task<bool> AddRatingAsync(int dishId, double rating)
        {
            var dish = await _dishRepository.GetByIdAsync(dishId);
            if (dish == null) return false;

            dish.RatingCount++;
            dish.Rating = (dish.Rating * (dish.RatingCount - 1) + rating) / dish.RatingCount;

            await _dishRepository.UpdateAsync(dish);
            return true;
        }

        public async Task<ItemRatingResponseDTO> GetRatingAsync(int dishId)
        {
            var dish = await _dishRepository.GetByIdAsync(dishId);
            if (dish == null) return null;

            return new ItemRatingResponseDTO
            {
                Id = dish.Id,
                Name = dish.Name,
                AverageRating = dish.Rating,
                RatingCount = dish.RatingCount
            };
        }
    }
}



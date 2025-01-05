using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MosEisleyCantina.Models.DTOs;
using MosEisleyCantinaAPI.Models.DTOs;
using MosEisleyCantinaAPI.Services.Interfaces;

namespace MosEisleyCantinaAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishesController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dishes = await _dishService.GetAllAsync();
            return Ok(dishes);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dish = await _dishService.GetByIdAsync(id);
            return Ok(dish);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DishDTO dish)
        {
            await _dishService.AddAsync(dish);
            return CreatedAtAction(nameof(GetById), new { id = dish.Name }, dish);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DishDTO dish)
        {
            await _dishService.UpdateAsync(id, dish);
            return Ok(new { Message = "Dish was updated successfully." });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _dishService.DeleteAsync(id);
            return Ok(new { Message = "Dish was deleted successfully." });
        }

        [Authorize(Roles = "User")]
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var results = await _dishService.SearchAsync(query);
            return Ok(results);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost("{dishId}/rate")]
        public async Task<IActionResult> RateDish(int dishId, [FromBody] RateItemDTO model)
        {
            if (!ModelState.IsValid || dishId != model.ItemId)
                return BadRequest();

            var success = await _dishService.AddRatingAsync(dishId, model.Rating);
            if (!success) return NotFound();

            return Ok(new { Message = "Rating added successfully." });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{dishId}/rating")]
        public async Task<IActionResult> GetDishRating(int dishId)
        {
            var rating = await _dishService.GetRatingAsync(dishId);
            if (rating == null) return NotFound();

            return Ok(rating);
        }
    }


}

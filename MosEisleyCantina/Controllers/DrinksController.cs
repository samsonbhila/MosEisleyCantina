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
    public class DrinksController : ControllerBase
    {
        private readonly IDrinkService _drinkService;

        public DrinksController(IDrinkService drinkService)
        {
            _drinkService = drinkService;
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var drinks = await _drinkService.GetAllAsync();
            return Ok(drinks);
        }

        [Authorize(Roles = "admin,user")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var drink = await _drinkService.GetByIdAsync(id);
            return Ok(drink);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DrinkDTO drink)
        {
            await _drinkService.AddAsync(drink);
            return CreatedAtAction(nameof(GetById), new { id = drink.Name }, drink);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DrinkDTO drink)
        {
            await _drinkService.UpdateAsync(id, drink);
            return Ok(new { Message = "Drink was updated successfully." });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _drinkService.DeleteAsync(id);
            return Ok(new { Message = "Drink was deleted successfully." });
        }

        [Authorize(Roles = "user")]
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var results = await _drinkService.SearchAsync(query);
            return Ok(results);
        }

        [Authorize(Roles = "user")]

        [HttpPost("{drinkId}/rate")]
        public async Task<IActionResult> RateDrink(int drinkId, [FromBody] RateItemDTO model)
        {
            if (!ModelState.IsValid || drinkId != model.ItemId)
                return BadRequest();

            var success = await _drinkService.AddRatingAsync(drinkId, model.Rating);
            if (!success) return NotFound();

            return Ok(new { Message = "Rating added successfully." });
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{drinkId}/rating")]
        public async Task<IActionResult> GetDrinkRating(int drinkId)
        {
            var rating = await _drinkService.GetRatingAsync(drinkId);
            if (rating == null) return NotFound();

            return Ok(rating);
        }
    }


}

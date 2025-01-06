using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MosEisleyCantinaAPI.Models.DTOs;
using MosEisleyCantinaAPI.Services.Interfaces;
using System.Security.Claims;

namespace MosEisleyCantinaAPI.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(IReviewService reviewService, UserManager<User> userManager, ILogger<ReviewController> logger)
        {
            _reviewService = reviewService;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll reviews request received.");
            try
            {
                var reviews = await _reviewService.GetAllReviewsAsync();
                _logger.LogInformation("Successfully retrieved all reviews.");
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while retrieving all reviews.");
                return StatusCode(500, "An error occurred while retrieving reviews.");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Get review request received for ID: {id}");
            try
            {
                var review = await _reviewService.GetReviewByIdAsync(id);
                if (review == null)
                {
                    _logger.LogWarning($"Review with ID {id} not found.");
                    return NotFound();
                }
                _logger.LogInformation($"Successfully retrieved review with ID: {id}");
                return Ok(review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong while retrieving review with ID: {id}");
                return StatusCode(500, "An error occurred while retrieving the review.");
            }
        }

        [Authorize(Roles = "admin,user")]
        [HttpPost]
        public async Task<IActionResult> Add(string content, int rating)
        {
            _logger.LogInformation("Add review request received.");
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger.LogWarning("Unauthorized attempt to add review.");
                    return Unauthorized();
                }

                _logger.LogInformation($"User {user.Email} is adding a review with rating {rating}.");
                var result = await _reviewService.AddReviewAsync(content, rating, user.Id);
                if (!result)
                {
                    _logger.LogWarning($"Failed to add review for user {user.Email}.");
                    return BadRequest("Failed to add review.");
                }

                _logger.LogInformation($"Review added successfully by user {user.Email}.");
                return Ok("Review added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while adding a review.");
                return StatusCode(500, "An error occurred while adding the review.");
            }
        }
    }
}

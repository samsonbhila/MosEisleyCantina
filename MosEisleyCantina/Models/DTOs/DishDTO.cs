using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace MosEisleyCantinaAPI.Models.DTOs
{
    public class DishDTO
    {
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name must be between 1 and 100 characters.", MinimumLength = 1)]
        public string? Name { get; set; }

        [StringLength(500, ErrorMessage = "The Description must be a maximum of 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The Price field is required.")]
        [Range(0.01, 10000, ErrorMessage = "The Price must be between 0.01 and 10,000.")]
        public decimal Price { get; set; }

        [Url(ErrorMessage = "The ImageUrl must be a valid URL.")]
        public string? ImageUrl { get; set; }
    }

}

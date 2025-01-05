using System.ComponentModel.DataAnnotations;

namespace MosEisleyCantina.Models.DTOs
{
    public class RateItemDTO
    {
        [Required]
        public int ItemId { get; set; }
        [Required]
        public double Rating { get; set; } 
    }

}

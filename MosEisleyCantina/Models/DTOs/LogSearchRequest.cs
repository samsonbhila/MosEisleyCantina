using System.ComponentModel.DataAnnotations;

namespace MosEisleyCantina.Models.DTOs
{
    public class LogSearchRequest
    {
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        public string? Level { get; set; }
        [Required]
        public string? SearchTerm { get; set; }
        [Required]
        public string? Message { get; set; }
    }
}

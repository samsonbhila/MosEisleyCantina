using System.ComponentModel.DataAnnotations;

namespace MosEisleyCantinaAPI.Models.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int Rating { get; set; }
        public string? UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}

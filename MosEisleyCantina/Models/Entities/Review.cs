using System.ComponentModel.DataAnnotations;

namespace MosEisleyCantinaAPI.Models.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Rating { get; set; }

        public string? UserId { get; set; } 
        public DateTime CreatedAt { get; set; }
        public User? User { get; set; }
    }
}

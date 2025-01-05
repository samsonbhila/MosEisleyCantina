namespace MosEisleyCantina.Models.DTOs
{
    public class ItemRatingResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }
    }

}

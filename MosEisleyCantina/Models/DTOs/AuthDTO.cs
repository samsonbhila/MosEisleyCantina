namespace MosEisleyCantinaAPI.Models.Responses
{
    public class AuthDTO
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public List<string>? Errors { get; set; }
    }
}

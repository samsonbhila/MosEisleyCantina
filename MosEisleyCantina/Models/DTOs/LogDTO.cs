using System.ComponentModel.DataAnnotations;

namespace MosEisleyCantina.Models.DTOs
{
    public class LogDto
    {
        
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string? Exception { get; set; }
        public string? LogEvent { get; set; }
        public string? LogLevel { get; set; }
    }

}

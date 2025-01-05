using System.ComponentModel.DataAnnotations;

namespace MosEisleyCantinaAPI.Models.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string? LastName { get; set; }

        [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string? UserName { get; set; }

        [MaxLength(20, ErrorMessage = "Role cannot exceed 20 characters.")]
        public string? Role { get; set; }
    }
}

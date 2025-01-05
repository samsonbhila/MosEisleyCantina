using MosEisleyCantinaAPI.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using MosEisleyCantinaAPI.Models.DTO;
using Google.Apis.Auth;

namespace MosEisleyCantinaAPI.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(RegisterDTO model);
        Task<string> LoginUserAsync(LoginDTO model, string ipAddress);
        Task<string> HandleExternalLogin(GoogleJsonWebSignature.Payload payload);
        Task<User> GetUserByEmailAsync(string email);
    }
}

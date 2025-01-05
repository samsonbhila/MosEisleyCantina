using Microsoft.AspNetCore.Identity;

namespace MosEisleyCantinaAPI.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(User user);
        
    }
}

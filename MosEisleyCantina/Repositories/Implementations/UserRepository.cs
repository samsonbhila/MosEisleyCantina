using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MosEisleyCantinaAPI.Data;
using MosEisleyCantinaAPI.Models;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task AddAsync(User user)
    {
        await _userManager.CreateAsync(user);
    }
}

using Microsoft.AspNetCore.Identity;

public interface IUserRepository
{
    Task<User> GetByEmailAsync(string email);
    Task AddAsync(User user); 
}

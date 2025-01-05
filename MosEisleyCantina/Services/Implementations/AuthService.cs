using Microsoft.AspNetCore.Identity;
using MosEisleyCantinaAPI.Interfaces;
using MosEisleyCantinaAPI.Models.DTOs;
using MosEisleyCantinaAPI.Services;
using MosEisleyCantinaAPI.Models.Responses;
using System.Security.Claims;
using MosEisleyCantinaAPI.Models.DTO;
using System.Text.RegularExpressions;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MosEisleyCantinaAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(IConfiguration configuration, IUserRepository userRepository,
                           IPasswordHasher<User> passwordHasher,
                           ITokenService tokenService,
                           UserManager<User> userManager,
                           RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return (User)await _userRepository.GetByEmailAsync(email);
        }

        public async Task<string> RegisterUserAsync(RegisterDTO model)
        {

            var existingUser = await _userRepository.GetByEmailAsync(model.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }

            if (!ValidatePasswordStrength(model.Password))
            {
                throw new Exception("Password does not meet the required strength criteria.");
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new Exception("User registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(model.Role));
                if (!roleResult.Succeeded)
                {
                    throw new Exception("Role creation failed: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }

            var roleAssignResult = await _userManager.AddToRoleAsync(user, model.Role);
            if (!roleAssignResult.Succeeded)
            {
                throw new Exception("Role assignment failed: " + string.Join(", ", roleAssignResult.Errors.Select(e => e.Description)));
            }

            return "User registered successfully.";
        }

        private bool ValidatePasswordStrength(string password)
        {
            var regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");

            return regex.IsMatch(password);
        }


        public async Task<string> LoginUserAsync(LoginDTO model, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new Exception("Invalid email or password.");
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                throw new Exception("Your account is locked due to multiple failed login attempts. Please try again later.");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                await _userManager.AccessFailedAsync(user);

                if (await _userManager.IsLockedOutAsync(user))
                {
                    throw new Exception("Too many failed attempts. Your account has been locked.");
                }

                throw new Exception("Invalid email or password.");
            }

            await _userManager.ResetAccessFailedCountAsync(user);

            return await _tokenService.GenerateToken(user);
        }
        public async Task<string> HandleExternalLogin(GoogleJsonWebSignature.Payload payload)
        {
            var user = await _userManager.FindByEmailAsync(payload.Email);

            if (user == null)
            {
                user = new User
                {
                    UserName = payload.Email,
                    Email = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName
                };

                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create user: " + string.Join(", ", result.Errors));
                }
            }

            var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

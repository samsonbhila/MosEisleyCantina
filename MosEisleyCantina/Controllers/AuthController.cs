using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MosEisleyCantinaAPI.Interfaces;
using MosEisleyCantinaAPI.Models.DTO;
using MosEisleyCantinaAPI.Models.DTOs;
using Microsoft.AspNetCore.RateLimiting;
using Google.Apis.Auth;
using MosEisleyCantinaAPI.Models.Responses;
using System;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace MosEisleyCantinaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, IConfiguration configuration, UserManager<User> userManager)
        {
            _authService = authService;
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Register attempt failed. Invalid model state.");
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.RegisterUserAsync(model);
                _logger.LogInformation($"User registered successfully: {model.Email}");
                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration.");
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login attempt failed. Invalid model state.");
                return BadRequest(ModelState);
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var deviceId = Request.Headers["Device-ID"].ToString();
            _logger.LogInformation($"Login attempt: User={model.Email}, Device-ID={deviceId}, IP={ipAddress}");

            try
            {
                var token = await _authService.LoginUserAsync(model, ipAddress);
                _logger.LogInformation($"User {model.Email} logged in successfully from IP {ipAddress}.");
                return Ok(new
                {
                    Token = token,
                    IPAddress = ipAddress
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login.");
                return Unauthorized(new
                {
                    Message = ex.Message,
                    IPAddress = ipAddress
                });
            }
        }

        [HttpPost("signin-google")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Google signin failed. Token is required.");
                return BadRequest(new { Message = "Token is required" });
            }

            try
            {
                var googleClientId = _configuration["Google:client_id"];
                var payload = await GoogleJsonWebSignature.ValidateAsync(token);

                if (payload == null)
                {
                    _logger.LogError("Invalid Google token.");
                    return Unauthorized(new { Message = "Invalid Google token." });
                }

                var user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new User
                    {
                        UserName = payload.Email,
                        Email = payload.Email
                    };

                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        _logger.LogError("User creation failed.");
                        return BadRequest(new { Message = "Failed to create user" });
                    }

                    _logger.LogInformation("New user created via Google signin: {Email}", payload.Email);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                        SecurityAlgorithms.HmacSha256Signature
                    )
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.CreateToken(tokenDescriptor);

                _logger.LogInformation($"Google signin successful for user: {user.Email}");
                return Ok(new { Token = tokenHandler.WriteToken(jwtToken) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during external login handling.");
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}

using DotnetAPI.Data;
using DotnetAPI.Models;
using DotNetAPI_EF.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DotnetAPI.Util;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(IConfiguration configuration) 
        {
            _authService = new AuthService(configuration);
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserForRegistrationDto user)
        {
            if (user.Password != user.PasswordConfirm) 
            {
                return BadRequest("Passwords don't match");
            }

            // Checks for existing user
            if (await _authService.UserExists(user.Email))
            {
                return BadRequest("Email already in use");
            }

            // Hashes password
            byte[] passwordSalt = _authService.GeneratePasswordSalt();

            byte[] passwordHash = _authService.GetPasswordHash(user.Password, passwordSalt);

            //Creates Auth User


            if (await _authService.CreateAuthUser(user.Email, passwordSalt, passwordHash))
            {
                throw new Exception("Error creating auth user!");
            }

            //Creates User
            

            if(!await _authService.CreateUser(user))
            {
                throw new Exception("Error registering user!");
            }

            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto user) 
        {

            if (!await _authService.ValidatePassword(user.Email, user.Password))
            {
                return Unauthorized("Invalid Password");
            }
            int userId = await _authService.GetUserId(user.Email);
            string token = _authService.CreateToken(userId);
            return Ok(new Dictionary<string, string>
            {
                {"token",token}
            });
        }

        [HttpGet("RefreshToken")]
        public async Task<string> RefreshToken()
        {
            string? authUserId = User.FindFirst("userId")?.Value;
            int userId = await _authService.GetValidatedUserId(authUserId ?? string.Empty);
            string newToken = _authService.CreateToken(userId);
            return newToken;
        }

        
    }
}

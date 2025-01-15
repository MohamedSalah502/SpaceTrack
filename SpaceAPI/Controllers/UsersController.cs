using Microsoft.AspNetCore.Mvc;
using SpaceTrack.DAL.Model;
using SpaceTrack.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using SpaceTrack.DAL.DTOs;

namespace SpaceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO dto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password); // Hash the password

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = hashedPassword // Save the hashed password
            };

            var result = await _userService.Register(user);
            if (result == null)
                return BadRequest("User already exists.");

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string name, string password)
        {

            var user = await _userService.Login(name, password);
            if (user == null)
                return Unauthorized("Invalid credentials. User not found.");
            
            // Generate JWT token if credentials are valid
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("JSAHGrinvfcroe480943kdsfskfpe??fti0943wjdwoiejfoj23484"); // Use a secure key
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

      ///  [Authorize]
        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsers());
        }
      //  [Authorize(Roles = "Admin")]
        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser(User user)
        {
            await _userService.AddUser(user);
            return Ok();
        }
    //    [Authorize]
        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Id))
            {
                return BadRequest("Invalid user data.");
            }

            await _userService.UpdateUser(user);
            return Ok("User updated successfully.");
        }
        
        //[Authorize]
        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUser(id);
            return Ok();
        }
    }
}



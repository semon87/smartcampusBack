using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Dev.Data;
using Dev.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System;

namespace Dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SmartCampusContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(SmartCampusContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginWithUserTypeDto dto)
        {
            if (dto.UserType.Equals("Student", StringComparison.OrdinalIgnoreCase))
            {
                var student = await _context.Users.SingleOrDefaultAsync(s => s.Email == dto.Email);
                if (student == null || student.Password != dto.Password)
                    return Unauthorized("Invalid email or password.");

                var token = GenerateJwtToken(student.UserId, student.Email, student.UserName, "Student");
                return Ok(new { token, userType = "Student" });
            }
            else if (dto.UserType.Equals("Vendor", StringComparison.OrdinalIgnoreCase))
            {
                var vendor = await _context.Vendors.SingleOrDefaultAsync(v => v.Email == dto.Email);
                if (vendor == null || vendor.Password != dto.Password)
                    return Unauthorized("Invalid email or password.");

                var token = GenerateJwtToken(vendor.VendorId, vendor.Email, vendor.VendorName, "Vendor");
                return Ok(new { token, userType = "Vendor" });
            }
            else
            {
                return BadRequest("Invalid user type.");
            }
        }

        private string GenerateJwtToken(int id, string email, string name, string userType)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, email),
        new Claim(ClaimTypes.Name, name),
        new Claim("UserType", userType)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

    public class LoginWithUserTypeDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string UserType { get; set; } = null!;  // "Student" or "Vendor"
    }


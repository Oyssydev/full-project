using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using ConverterAPI.Data;
using ConverterClassLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ConverterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public DataContext _context { get; }

        public AuthController(IConfiguration configuration, DataContext context)
        {
            this.configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            User userTest = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if(userTest != null)
            {
                return BadRequest("Username : " + request.Username + " already exists" );
            }

            User user = new User(); // Instantiate the User object
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;


            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registered successfully" });
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt) || user == null)
            {
                return BadRequest("Username or password invalid");
            }
            string token = CreateToken(user);
            return Ok(token);
        }




        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash ,byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                ) ;

            var jwt = new JwtSecurityTokenHandler().WriteToken(token) ;
            return jwt;
        }
    }
}

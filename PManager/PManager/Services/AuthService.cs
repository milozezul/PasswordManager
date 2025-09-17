using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PManager.Data;
using PManager.Interfaces.Services;
using PManager.Models.Configs;
using PManager.Models.Database;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PManager.Services
{
    public class AuthService : IAuthService
    {
        PManagerDbContext _context;
        IOptions<JWTConfigs> _config;
        public AuthService(PManagerDbContext context, IOptions<JWTConfigs> config)
        {
            _context = context;
            _config = config;
        }

        async Task<User> GetUser(string username)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Username == username)
                    .FirstAsync();
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string?> Login(string username, string password)
        {
            var user = await GetUser(username);
            if (user != null)
            {
                using (var sha = SHA256.Create())
                {
                    if (user.PasswordHash == Encoding.UTF8.GetString(sha.ComputeHash(Encoding.UTF8.GetBytes(password))))
                    {
                        return GetBearerToken(user.Id);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<User> CreateUser(string username, string password)
        {
            try
            {
                using (var sha = SHA256.Create())
                {
                    var user = await _context.Users.AddAsync(new User()
                    {
                        Username = username,
                        PasswordHash = Encoding.UTF8.GetString(sha.ComputeHash(Encoding.UTF8.GetBytes(password)))
                    });
                    return user.Entity;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        string GetBearerToken(int id)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, id.ToString())
            };

            string strKey = _config.Value.Secret;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(strKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: cred
            );

            string bearer = new JwtSecurityTokenHandler().WriteToken(token);
            return bearer;
        }
    }
}

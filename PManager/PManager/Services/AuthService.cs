using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PManager.Data;
using PManager.Interfaces.Services;
using PManager.Models.Configs;
using PManager.Models.Database;
using SharedModels.InputModels;
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

        async Task<User?> GetUser(string username)
        {
            return await _context.Users
                .SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<LoginResponse> Login(LoginInput input)
        {
            var user = await GetUser(input.Username);

            if (user == null) return new LoginResponse() {
                IsSuccess = false,
                Token = "",
                Message = "User not found."
            };
            
            using (var sha = SHA256.Create())
            {
                var passwordHash = Encoding.UTF8.GetString(sha.ComputeHash(Encoding.UTF8.GetBytes(input.Password)));

                if (user.PasswordHash != passwordHash) return new LoginResponse()
                {
                    IsSuccess = false,
                    Token = "",
                    Message = "Wrong password."
                };

                return new LoginResponse()
                {
                    IsSuccess = true,
                    Token = GetBearerToken(user.Id),
                    Message = "Success"
                };
            }
        }

        public async Task<User?> CreateUser(LoginInput input)
        {
            try
            {
                using (var sha = SHA256.Create())
                {
                    var user = await _context.Users.AddAsync(new User()
                    {
                        Username = input.Username,
                        PasswordHash = Encoding.UTF8.GetString(sha.ComputeHash(Encoding.UTF8.GetBytes(input.Password)))
                    });
                    await _context.SaveChangesAsync();
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
                new Claim(ClaimTypes.Sid, id.ToString()),
                new Claim(ClaimTypes.Role, "User")
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
